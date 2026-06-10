using System.Text.Json;
using Terminal.Gui;

var installer = new InstallerApp();
installer.Run();

internal sealed class InstallerApp
{
    private const string RepoOwner = "sim2kid";
    private const string RepoName = "agentic-skills";
    private const string StateFile = ".agents/agent-packages-installed.json";
    private const string DefaultAgentType = "OpenCode";

    private readonly HttpClient _httpClient = new();
    private readonly string _workspaceRoot = Environment.CurrentDirectory;

    private string _agentType = DefaultAgentType;
    private string _selectedVersion = "latest";

    public void Run()
    {
        Application.Init();

        try
        {
            ShowMainMenu();
            Application.Run();
        }
        finally
        {
            Application.Shutdown();
            _httpClient.Dispose();
        }
    }

    private void ShowMainMenu()
    {
        var state = LoadState();
        var currentVersion = state?.Version ?? "None";
        var currentAgent = state?.AgentType ?? "None";
        var actionLabel = currentVersion == "None" ? "Install" : "Install / Update";

        var window = CreateCenteredWindow("Agentic Skills Installer", 78, 20);
        AddHeader(window, currentVersion, currentAgent);

        var installButton = new Button("_Install / Update")
        {
            X = Pos.Center() - 18,
            Y = 10,
            IsDefault = true
        };
        installButton.Clicked += () => StartInstallWizard();

        var uninstallButton = new Button("_Uninstall")
        {
            X = Pos.Center() + 4,
            Y = 10
        };
        uninstallButton.Clicked += RunUninstallFlow;

        var quitButton = new Button("_Quit")
        {
            X = Pos.Center() - 4,
            Y = 13
        };
        quitButton.Clicked += () => Application.RequestStop();

        window.Add(
            new Label("Use keyboard shortcuts or mouse to continue.")
            {
                X = Pos.Center() - 21,
                Y = 8
            },
            installButton,
            uninstallButton,
            quitButton);

        ReplaceTop(window);
    }

    private void StartInstallWizard()
    {
        var agentType = SelectAgentType();
        if (agentType is null)
        {
            return;
        }

        _agentType = agentType;

        var version = SelectVersion();
        if (version is null)
        {
            return;
        }

        _selectedVersion = version;

        PackageMetadata metadata;
        try
        {
            metadata = GetPackageMetadata(version);
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
            var previousState = LoadState();
            if (previousState is not null)
            {
                RemovePreviouslyInstalledPackages(previousState);
            }

            var savedVersion = version == "latest" ? GetLocalVersionLabel() : version;
            var newState = new InstalledState(savedVersion, _agentType, selectedPackages, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            SaveState(newState);
            DeployPackages(version, selectedPackages);

            MessageBox.Query("Installer", $"Installation complete.\nVersion: {savedVersion}\nAgent: {_agentType}", "OK");
            ShowMainMenu();
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Installer", ex.Message, "OK");
        }
    }

    private void RunUninstallFlow()
    {
        if (MessageBox.Query("Uninstall", "Remove installed skills and agents?", "_Yes", "_No") != 0)
        {
            return;
        }

        try
        {
            var state = LoadState();
            if (state is not null)
            {
                RemovePreviouslyInstalledPackages(state);
            }

            var statePath = Path.Combine(_workspaceRoot, StateFile);
            if (File.Exists(statePath))
            {
                File.Delete(statePath);
            }

            MessageBox.Query("Uninstall", "Successfully uninstalled all packages.", "OK");
            ShowMainMenu();
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Uninstall", ex.Message, "OK");
        }
    }

    private string? SelectAgentType()
    {
        var dialog = CreateCenteredDialog("Select Agent Type", 60, 16);
        var options = new List<string> { DefaultAgentType };
        var listView = new ListView(options)
        {
            X = 2,
            Y = 4,
            Width = Dim.Fill() - 4,
            Height = 4
        };

        dialog.Add(CreateHeaderView("Agent Type", LoadState()?.Version ?? "None", _agentType));
        dialog.Add(new Label("Choose the agent platform to install into.") { X = Pos.Center() - 20, Y = 2 });
        dialog.Add(listView);

        string? selected = null;
        var nextButton = new Button("_Next", is_default: true);
        nextButton.Clicked += () =>
        {
            selected = options[listView.SelectedItem];
            Application.RequestStop();
        };

        var cancelButton = new Button("_Cancel");
        cancelButton.Clicked += () => Application.RequestStop();

        dialog.AddButton(nextButton);
        dialog.AddButton(cancelButton);
        Application.Run(dialog);
        return selected;
    }

    private string? SelectVersion()
    {
        var versions = GetVersions();
        var dialog = CreateCenteredDialog("Select Version", 72, 20);
        var listView = new ListView(versions)
        {
            X = 2,
            Y = 4,
            Width = Dim.Fill() - 4,
            Height = 9
        };

        dialog.Add(CreateHeaderView("Version", LoadState()?.Version ?? "None", _agentType));
        dialog.Add(new Label("Choose `latest` or a specific tag.") { X = Pos.Center() - 17, Y = 2 });
        dialog.Add(listView);

        string? selected = null;
        var nextButton = new Button("_Next", is_default: true);
        nextButton.Clicked += () =>
        {
            selected = versions[listView.SelectedItem];
            Application.RequestStop();
        };

        var backButton = new Button("_Back");
        backButton.Clicked += () => Application.RequestStop();

        dialog.AddButton(nextButton);
        dialog.AddButton(backButton);
        Application.Run(dialog);
        return selected;
    }

    private List<string>? SelectPackages(PackageMetadata metadata)
    {
        var previous = LoadState()?.Packages ?? new List<string>();
        var dialog = CreateCenteredDialog("Select Packages", 96, Math.Min(26, metadata.Packages.Count + 10));
        dialog.Add(CreateHeaderView("Packages", LoadState()?.Version ?? "None", _agentType));
        dialog.Add(new Label($"Version: {_selectedVersion}") { X = 2, Y = 2 });
        dialog.Add(new Label("Use space to toggle packages. Enter activates the default button.") { X = 2, Y = 3 });

        var checkBoxes = new List<CheckBox>();
        for (var i = 0; i < metadata.Packages.Count; i++)
        {
            var package = metadata.Packages[i];
            var checkBox = new CheckBox(2, i + 5, package.Id)
            {
                Checked = previous.Count > 0 ? previous.Contains(package.Id) : true
            };
            dialog.Add(checkBox);
            dialog.Add(new Label(28, i + 5, package.Description));
            checkBoxes.Add(checkBox);
        }

        List<string>? selected = null;
        var installButton = new Button("_Install", is_default: true);
        installButton.Clicked += () =>
        {
            selected = metadata.Packages
                .Where((pkg, index) => checkBoxes[index].Checked)
                .Select(pkg => pkg.Id)
                .ToList();
            Application.RequestStop();
        };

        var backButton = new Button("_Back");
        backButton.Clicked += () => Application.RequestStop();

        dialog.AddButton(installButton);
        dialog.AddButton(backButton);
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

            using var commitRequest = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{RepoOwner}/{RepoName}/commits/main");
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
            response.Dispose();
            using var fallbackRequest = new HttpRequestMessage(HttpMethod.Get, $"https://raw.githubusercontent.com/{RepoOwner}/{RepoName}/main/packages.json");
            fallbackRequest.Headers.Add("User-Agent", "AgenticSkillsInstaller");
            var fallbackResponse = _httpClient.Send(fallbackRequest);
            fallbackResponse.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<PackageMetadata>(fallbackResponse.Content.ReadAsStringAsync().Result, JsonOptions())
                ?? throw new InvalidOperationException("Failed to parse packages.json from main.");
        }

        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<PackageMetadata>(response.Content.ReadAsStringAsync().Result, JsonOptions())
            ?? throw new InvalidOperationException("Failed to parse packages.json.");
    }

    private void RemovePreviouslyInstalledPackages(InstalledState state)
    {
        if (!string.Equals(state.AgentType, DefaultAgentType, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Unsupported agent type in state file: {state.AgentType}");
        }

        var skillsBase = Path.Combine(_workspaceRoot, ".opencode", "skills");
        foreach (var packageId in state.Packages)
        {
            var installedPackageDir = Path.Combine(skillsBase, packageId);
            if (Directory.Exists(installedPackageDir))
            {
                Directory.Delete(installedPackageDir, true);
            }
        }

        var opencodePath = Path.Combine(_workspaceRoot, ".opencode");
        if (Directory.Exists(opencodePath))
        {
            var skillsPath = Path.Combine(opencodePath, "skills");
            var agentsPath = Path.Combine(opencodePath, "agents");
            if (Directory.Exists(skillsPath) && !Directory.EnumerateFileSystemEntries(skillsPath).Any())
            {
                Directory.Delete(skillsPath, true);
            }

            if (Directory.Exists(agentsPath) && !Directory.EnumerateFileSystemEntries(agentsPath).Any())
            {
                Directory.Delete(agentsPath, true);
            }

            if (!Directory.EnumerateFileSystemEntries(opencodePath).Any())
            {
                Directory.Delete(opencodePath, true);
            }
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
                    throw new InvalidOperationException($"Package directory not found: {packageId}");
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
        using var request = new HttpRequestMessage(HttpMethod.Get, $"https://github.com/{RepoOwner}/{RepoName}/archive/refs/heads/{branch}.zip");
        request.Headers.Add("User-Agent", "AgenticSkillsInstaller");
        var response = _httpClient.Send(request);

        if (!response.IsSuccessStatusCode && branch != "main")
        {
            response.Dispose();
            using var fallbackRequest = new HttpRequestMessage(HttpMethod.Get, $"https://github.com/{RepoOwner}/{RepoName}/archive/refs/tags/{branch}.zip");
            fallbackRequest.Headers.Add("User-Agent", "AgenticSkillsInstaller");
            var fallbackResponse = _httpClient.Send(fallbackRequest);
            fallbackResponse.EnsureSuccessStatusCode();
            File.WriteAllBytes(zipPath, fallbackResponse.Content.ReadAsByteArrayAsync().Result);
        }
        else
        {
            response.EnsureSuccessStatusCode();
            File.WriteAllBytes(zipPath, response.Content.ReadAsByteArrayAsync().Result);
        }

        System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, tempDir, true);
        return Directory.GetDirectories(tempDir)
            .First(path => !string.Equals(Path.GetFileName(path), ".git", StringComparison.OrdinalIgnoreCase));
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

    private Window CreateCenteredWindow(string title, int width, int height)
    {
        return new Window(title)
        {
            X = Pos.Center(),
            Y = Pos.Center(),
            Width = width,
            Height = height
        };
    }

    private Dialog CreateCenteredDialog(string title, int width, int height)
    {
        return new Dialog(title, width, height)
        {
            X = Pos.Center(),
            Y = Pos.Center()
        };
    }

    private void AddHeader(Window window, string currentVersion, string currentAgent)
    {
        window.Add(CreateHeaderView("Main Menu", currentVersion, currentAgent));
    }

    private View CreateHeaderView(string stepName, string currentVersion, string currentAgent)
    {
        var frame = new FrameView()
        {
            X = 1,
            Y = 0,
            Width = Dim.Fill() - 2,
            Height = 5,
            CanFocus = false,
            Title = stepName
        };

        frame.Add(new Label($"Agentic Skills Installer") { X = 2, Y = 0 });
        frame.Add(new Label($"Installed Version: {currentVersion}") { X = 2, Y = 1 });
        frame.Add(new Label($"Agent Type: {currentAgent}") { X = 34, Y = 1 });
        frame.Add(new Label($"Repo: {RepoOwner}/{RepoName}") { X = 2, Y = 2 });
        return frame;
    }

    private void ReplaceTop(View view)
    {
        Application.Top.RemoveAll();
        Application.Top.Add(view);
    }

    private static void CopyDirectory(string sourceDir, string destinationDir)
    {
        if (Directory.Exists(destinationDir))
        {
            Directory.Delete(destinationDir, true);
        }

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
}

internal sealed record GitTag(string Name);
internal sealed record GitCommit(string Sha);
internal sealed record InstalledState(string Version, string AgentType, List<string> Packages, string Timestamp);
internal sealed record PackageMetadata(List<PackageDefinition> Packages);
internal sealed record PackageDefinition(string Id, string Description, List<string> Dependencies);
