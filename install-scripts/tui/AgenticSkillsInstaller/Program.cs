using System.Diagnostics;
using System.Text.Json;
using Terminal.Gui;

var installer = new InstallerApp();
installer.Run();

internal sealed class InstallerApp
{
    private const string RepoOwner = "sim2kid";
    private const string RepoName = "agentic-skills";
    private const string StateFile = ".agents/agent-packages-installed.json";

    private readonly HttpClient _httpClient = new();
    private readonly string _workspaceRoot = FindWorkspaceRoot();

    public void Run()
    {
        Application.Init();

        try
        {
            ShowMainWindow();
            Application.Run();
        }
        finally
        {
            Application.Shutdown();
            _httpClient.Dispose();
        }
    }

    private void ShowMainWindow()
    {
        var state = LoadState();
        var currentVersion = state?.Version ?? "None";
        var actionLabel = currentVersion == "None" ? "Install" : "Install / Update";

        var window = new Window("Agentic Skills Installer")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        window.Add(new Label(2, 1, "Agentic Skills Installation"));
        window.Add(new Label(2, 3, $"Target Repo: {RepoOwner}/{RepoName}"));
        window.Add(new Label(2, 4, $"Workspace: {_workspaceRoot}"));
        window.Add(new Label(2, 5, $"Current Installed Version: {currentVersion}"));

        var installButton = new Button(2, 8, actionLabel);
        installButton.Clicked += () => RunInstallFlow(window);

        var uninstallButton = new Button(24, 8, "Uninstall");
        uninstallButton.Clicked += () => RunUninstallFlow();

        var quitButton = new Button(2, 12, "Quit");
        quitButton.Clicked += () => Application.RequestStop();

        window.Add(installButton, uninstallButton, quitButton);
        Application.Top.RemoveAll();
        Application.Top.Add(window);
    }

    private void RunInstallFlow(Window parent)
    {
        var selectedVersion = SelectVersion();
        if (selectedVersion is null)
        {
            return;
        }

        PackageMetadata metadata;
        try
        {
            metadata = GetPackageMetadata(selectedVersion);
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Installer", ex.Message, "OK");
            return;
        }

        var selectedPackages = SelectPackages(metadata);
        if (selectedPackages is null || selectedPackages.Count == 0)
        {
            return;
        }

        try
        {
            CleanupExisting();
            DeployPackages(selectedVersion, selectedPackages);

            var localVersion = GetLocalVersionLabel();
            var displayVersion = selectedVersion == "latest" ? $"latest ({localVersion})" : selectedVersion;
            SaveState(new InstalledState(displayVersion, "OpenCode", selectedPackages, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

            MessageBox.Query("Installer", $"Installation complete.\nInstalled version: {displayVersion}", "OK");
            ShowMainWindow();
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Installer", ex.Message, "OK");
        }
    }

    private void RunUninstallFlow()
    {
        if (MessageBox.Query("Uninstall", "Remove installed skills and agents?", "Yes", "No") != 0)
        {
            return;
        }

        try
        {
            var opencodePath = Path.Combine(_workspaceRoot, ".opencode");
            if (Directory.Exists(opencodePath))
            {
                Directory.Delete(opencodePath, true);
            }

            var statePath = Path.Combine(_workspaceRoot, StateFile);
            if (File.Exists(statePath))
            {
                File.Delete(statePath);
            }

            MessageBox.Query("Uninstall", "Successfully uninstalled all packages.", "OK");
            ShowMainWindow();
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Uninstall", ex.Message, "OK");
        }
    }

    private string? SelectVersion()
    {
        var versions = GetVersions();
        var dialog = new Dialog("Select Version", 70, 20);
        var listView = new ListView(versions)
        {
            X = 1,
            Y = 1,
            Width = Dim.Fill() - 2,
            Height = Dim.Fill() - 4
        };

        string? selected = null;
        var okButton = new Button("OK", is_default: true);
        okButton.Clicked += () =>
        {
            selected = versions[listView.SelectedItem];
            Application.RequestStop();
        };

        var cancelButton = new Button("Cancel");
        cancelButton.Clicked += () => Application.RequestStop();

        dialog.Add(listView);
        dialog.AddButton(okButton);
        dialog.AddButton(cancelButton);

        Application.Run(dialog);
        return selected;
    }

    private List<string>? SelectPackages(PackageMetadata metadata)
    {
        var previous = LoadState()?.Packages ?? new List<string>();
        var dialog = new Dialog("Select Packages", 90, 24);
        var checkBoxes = new List<CheckBox>();

        dialog.Add(new Label(1, 1, "Toggle packages with mouse or keyboard. Dependencies are not auto-resolved yet."));

        for (var i = 0; i < metadata.Packages.Count; i++)
        {
            var package = metadata.Packages[i];
            var checkBox = new CheckBox(1, i + 3, package.Id)
            {
                Checked = previous.Count > 0 ? previous.Contains(package.Id) : true
            };
            dialog.Add(checkBox);
            dialog.Add(new Label(24, i + 3, package.Description));
            checkBoxes.Add(checkBox);
        }

        List<string>? selected = null;
        var okButton = new Button("Install", is_default: true);
        okButton.Clicked += () =>
        {
            selected = metadata.Packages
                .Where((pkg, index) => checkBoxes[index].Checked)
                .Select(pkg => pkg.Id)
                .ToList();
            Application.RequestStop();
        };

        var cancelButton = new Button("Cancel");
        cancelButton.Clicked += () => Application.RequestStop();

        dialog.AddButton(okButton);
        dialog.AddButton(cancelButton);

        Application.Run(dialog);
        return selected;
    }

    private List<string> GetVersions()
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{RepoOwner}/{RepoName}/tags?per_page=100");
            request.Headers.Add("User-Agent", "AgenticSkillsInstaller");
            var response = _httpClient.Send(request);
            response.EnsureSuccessStatusCode();

            var tags = JsonSerializer.Deserialize<List<GitTag>>(response.Content.ReadAsStringAsync().Result, JsonOptions()) ?? new List<GitTag>();
            var versions = new List<string> { "latest" };
            versions.AddRange(tags.Select(tag => tag.Name));
            return versions;
        }
        catch
        {
            return new List<string> { "latest" };
        }
    }

    private string GetLocalVersionLabel()
    {
        const string fallbackTag = "0.1.0";

        try
        {
            using var tagRequest = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{RepoOwner}/{RepoName}/tags?per_page=1");
            tagRequest.Headers.Add("User-Agent", "AgenticSkillsInstaller");
            var tagResponse = _httpClient.Send(tagRequest);
            tagResponse.EnsureSuccessStatusCode();
            var tags = JsonSerializer.Deserialize<List<GitTag>>(tagResponse.Content.ReadAsStringAsync().Result, JsonOptions()) ?? new List<GitTag>();

            using var commitRequest = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{RepoOwner}/{RepoName}/commits/main?per_page=1");
            commitRequest.Headers.Add("User-Agent", "AgenticSkillsInstaller");
            var commitResponse = _httpClient.Send(commitRequest);
            commitResponse.EnsureSuccessStatusCode();
            var commit = JsonSerializer.Deserialize<GitCommit>(commitResponse.Content.ReadAsStringAsync().Result, JsonOptions());

            var latestTag = tags.FirstOrDefault()?.Name ?? fallbackTag;
            var shortSha = commit?.Sha is { Length: >= 6 } sha ? sha[..6] : "000000";
            return $"{latestTag}+{shortSha}";
        }
        catch
        {
            return fallbackTag;
        }
    }

    private PackageMetadata GetPackageMetadata(string version)
    {
        if (Environment.GetEnvironmentVariable("LOCAL_TEST_MODE") == "true")
        {
            var localPath = Path.Combine(_workspaceRoot, "packages.json");
            return JsonSerializer.Deserialize<PackageMetadata>(File.ReadAllText(localPath), JsonOptions())
                ?? throw new InvalidOperationException("Failed to read local packages.json.");
        }

        var branch = version == "latest" ? "main" : version;
        var url = $"https://raw.githubusercontent.com/{RepoOwner}/{RepoName}/{branch}/packages.json";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("User-Agent", "AgenticSkillsInstaller");
        var response = _httpClient.Send(request);

        if (!response.IsSuccessStatusCode && branch != "main")
        {
            using var fallbackRequest = new HttpRequestMessage(HttpMethod.Get, $"https://raw.githubusercontent.com/{RepoOwner}/{RepoName}/main/packages.json");
            fallbackRequest.Headers.Add("User-Agent", "AgenticSkillsInstaller");
            response.Dispose();
            var fallbackResponse = _httpClient.Send(fallbackRequest);
            fallbackResponse.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<PackageMetadata>(fallbackResponse.Content.ReadAsStringAsync().Result, JsonOptions())
                ?? throw new InvalidOperationException("Failed to parse packages.json from main.");
        }

        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<PackageMetadata>(response.Content.ReadAsStringAsync().Result, JsonOptions())
            ?? throw new InvalidOperationException("Failed to parse packages.json.");
    }

    private void CleanupExisting()
    {
        var skillsPath = Path.Combine(_workspaceRoot, ".opencode", "skills");
        var agentsPath = Path.Combine(_workspaceRoot, ".opencode", "agents");

        if (Directory.Exists(skillsPath))
        {
            Directory.Delete(skillsPath, true);
        }

        if (Directory.Exists(agentsPath))
        {
            Directory.Delete(agentsPath, true);
        }
    }

    private void DeployPackages(string version, IReadOnlyCollection<string> selectedPackages)
    {
        var branch = version == "latest" ? "main" : version;
        var tempDir = Path.Combine(Path.GetTempPath(), $"agentic-skills-install-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempDir);

        try
        {
            var sourceRoot = Environment.GetEnvironmentVariable("LOCAL_TEST_MODE") == "true"
                ? _workspaceRoot
                : DownloadAndExtractSource(branch, tempDir);

            foreach (var packageId in selectedPackages)
            {
                var packageDir = Path.Combine(sourceRoot, "packages", packageId);
                if (!Directory.Exists(packageDir))
                {
                    continue;
                }

                CopySkills(packageDir);
                CopyAgents(packageDir);
            }
        }
        finally
        {
            if (Directory.Exists(tempDir) && Environment.GetEnvironmentVariable("LOCAL_TEST_MODE") != "true")
            {
                Directory.Delete(tempDir, true);
            }
        }
    }

    private string DownloadAndExtractSource(string branch, string tempDir)
    {
        var zipPath = Path.Combine(tempDir, "source.zip");
        using var request = new HttpRequestMessage(HttpMethod.Get, $"https://github.com/{RepoOwner}/{RepoName}/archive/{branch}.zip");
        request.Headers.Add("User-Agent", "AgenticSkillsInstaller");
        var response = _httpClient.Send(request);
        response.EnsureSuccessStatusCode();
        File.WriteAllBytes(zipPath, response.Content.ReadAsByteArrayAsync().Result);

        System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, tempDir, true);
        return Directory.GetDirectories(tempDir).First(static path => Path.GetFileName(path) != ".git");
    }

    private void CopySkills(string packageDir)
    {
        var skillsSrc = Path.Combine(packageDir, "skills");
        if (!Directory.Exists(skillsSrc))
        {
            return;
        }

        var skillsDestBase = Path.Combine(_workspaceRoot, ".opencode", "skills");
        Directory.CreateDirectory(skillsDestBase);

        foreach (var skillDir in Directory.GetDirectories(skillsSrc))
        {
            var skillDest = Path.Combine(skillsDestBase, Path.GetFileName(skillDir));
            CopyDirectory(skillDir, skillDest);
        }
    }

    private void CopyAgents(string packageDir)
    {
        var agentsSrc = Path.Combine(packageDir, "sub-agents");
        if (!Directory.Exists(agentsSrc))
        {
            return;
        }

        var agentsDestBase = Path.Combine(_workspaceRoot, ".opencode", "agents");
        Directory.CreateDirectory(agentsDestBase);

        foreach (var agentDir in Directory.GetDirectories(agentsSrc))
        {
            var agentFile = Path.Combine(agentDir, "AGENT.md");
            if (!File.Exists(agentFile))
            {
                continue;
            }

            var agentDest = Path.Combine(agentsDestBase, $"{Path.GetFileName(agentDir)}.md");
            File.Copy(agentFile, agentDest, true);
        }
    }

    private InstalledState? LoadState()
    {
        var statePath = Path.Combine(_workspaceRoot, StateFile);
        if (!File.Exists(statePath))
        {
            return null;
        }

        return JsonSerializer.Deserialize<InstalledState>(File.ReadAllText(statePath), JsonOptions());
    }

    private void SaveState(InstalledState state)
    {
        var statePath = Path.Combine(_workspaceRoot, StateFile);
        Directory.CreateDirectory(Path.GetDirectoryName(statePath)!);
        File.WriteAllText(statePath, JsonSerializer.Serialize(state, JsonOptions()));
    }

    private static void CopyDirectory(string sourceDir, string destinationDir)
    {
        Directory.CreateDirectory(destinationDir);

        foreach (var file in Directory.GetFiles(sourceDir))
        {
            File.Copy(file, Path.Combine(destinationDir, Path.GetFileName(file)), true);
        }

        foreach (var directory in Directory.GetDirectories(sourceDir))
        {
            CopyDirectory(directory, Path.Combine(destinationDir, Path.GetFileName(directory)));
        }
    }

    private static JsonSerializerOptions JsonOptions() => new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    private static string FindWorkspaceRoot()
    {
        var current = AppContext.BaseDirectory;
        while (!string.IsNullOrEmpty(current))
        {
            if (File.Exists(Path.Combine(current, "packages.json")))
            {
                return current;
            }

            current = Directory.GetParent(current)?.FullName ?? string.Empty;
        }

        return Environment.CurrentDirectory;
    }
}

internal sealed record GitTag(string Name);
internal sealed record GitCommit(string Sha);
internal sealed record InstalledState(string Version, string AgentType, List<string> Packages, string Timestamp);
internal sealed record PackageMetadata(List<PackageDefinition> Packages);
internal sealed record PackageDefinition(string Id, string Description, List<string> Dependencies);
