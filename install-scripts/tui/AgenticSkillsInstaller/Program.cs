using System.Collections;
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
    private const int ShellWidth = 100;
    private const int ShellHeight = 30;
    private const int ButtonRowY = 22;

    private readonly HttpClient _httpClient = new();
    private readonly string _workspaceRoot = Environment.CurrentDirectory;

    private Window? _window;
    private FrameView? _infoFrame;
    private FrameView? _contentFrame;
    private string _selectedAgentType = DefaultAgentType;
    private string _selectedVersion = "latest";
    private List<string> _selectedPackages = new();
    private List<PackageDefinition> _availablePackages = new();
    private int _agentTypeIndex;
    private int _versionIndex;

    public void Run()
    {
        Console.CancelKeyPress += HandleCancelKeyPress;
        Application.Init();

        try
        {
            BuildShell();
            ShowMainMenu();
            Application.Run();
        }
        finally
        {
            Console.CancelKeyPress -= HandleCancelKeyPress;
            Application.Shutdown();
            _httpClient.Dispose();
        }
    }

    private static void HandleCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        e.Cancel = true;
        Application.RequestStop();
    }

    private void BuildShell()
    {
        _window = new Window("Agentic Skills Installer")
        {
            X = Pos.Center(),
            Y = Pos.Center(),
            Width = ShellWidth,
            Height = ShellHeight
        };

        _window.KeyPress += HandleGlobalKeyPress;

        _infoFrame = new FrameView("Info")
        {
            X = 1,
            Y = 0,
            Width = Dim.Fill() - 2,
            Height = 5,
            CanFocus = false
        };

        _contentFrame = new FrameView()
        {
            X = 1,
            Y = Pos.Bottom(_infoFrame),
            Width = Dim.Fill() - 2,
            Height = Dim.Fill() - 1
        };

        _window.Add(_infoFrame, _contentFrame);
        Application.Top.RemoveAll();
        Application.Top.Add(_window);
    }

    private void HandleGlobalKeyPress(View.KeyEventEventArgs args)
    {
        if (args.KeyEvent.Key == (Key.CtrlMask | Key.C))
        {
            args.Handled = true;
            Application.RequestStop();
        }
    }

    private void UpdateInfoFrame()
    {
        if (_infoFrame is null)
        {
            return;
        }

        _infoFrame.RemoveAll();
        var state = LoadState();
        _infoFrame.Add(new Label(2, 0, $"Installed Version: {state?.Version ?? "None"}"));
        _infoFrame.Add(new Label(40, 0, $"Installed Agent Type: {state?.AgentType ?? "None"}"));
        _infoFrame.Add(new Label(2, 1, $"Selected Agent Type: {_selectedAgentType}"));
        _infoFrame.Add(new Label(40, 1, $"Selected Version: {_selectedVersion}"));
        _infoFrame.Add(new Label(2, 2, $"Repo: {RepoOwner}/{RepoName}"));
    }

    private void ReplaceContent(string title, params View[] views)
    {
        if (_contentFrame is null)
        {
            return;
        }

        _contentFrame.Title = title;
        _contentFrame.RemoveAll();
        foreach (var view in views)
        {
            _contentFrame.Add(view);
        }

        UpdateInfoFrame();
        SetInitialFocus();
    }

    private void SetInitialFocus()
    {
        if (_contentFrame is null)
        {
            return;
        }

        _contentFrame.FocusFirst();
    }

    private void ShowMainMenu()
    {
        var state = LoadState();

        _window!.KeyPress -= MainMenuKeyPress;
        _window.KeyPress += MainMenuKeyPress;

        ReplaceContent(
            "Welcome",
            new Label(28, 4, "Choose what you want to do."),
            new Label(28, 5, "Hotkeys: I = Install/Update, U = Uninstall, Ctrl+C = Exit"),
            new Label(28, 6, state is null ? "No previous installation found." : "Existing installation detected."),
            CreateButton(28, ButtonRowY, "_Install / Update", StartInstallWizard),
            CreateButton(52, ButtonRowY, "_Uninstall", RunUninstallFlow));
    }

    private void MainMenuKeyPress(View.KeyEventEventArgs args)
    {
        if (args.KeyEvent.Key == Key.I || args.KeyEvent.Key == Key.i)
        {
            args.Handled = true;
            StartInstallWizard();
        }

        if (args.KeyEvent.Key == Key.U || args.KeyEvent.Key == Key.u)
        {
            args.Handled = true;
            RunUninstallFlow();
        }
    }

    private void StartInstallWizard()
    {
        _window!.KeyPress -= MainMenuKeyPress;
        ShowAgentTypeStep();
    }

    private void ShowAgentTypeStep()
    {
        var options = new List<string> { DefaultAgentType };
        var listView = CreateListView(options, _agentTypeIndex, 4, 12);
        listView.SelectedItemChanged += args => _agentTypeIndex = args.Item;

        _window!.KeyPress -= AgentTypeKeyPress;
        _window.KeyPress += AgentTypeKeyPress;

        ReplaceContent(
            "Agent Type",
            new Label(2, 1, "Select the agent platform you want to install these packages into."),
            new Label(2, 2, "Available agent types:"),
            listView,
            CreateButton(32, ButtonRowY, "_Next", () =>
            {
                _selectedAgentType = options[listView.SelectedItem];
                _agentTypeIndex = listView.SelectedItem;
                _window!.KeyPress -= AgentTypeKeyPress;
                ShowVersionStep();
            }),
            CreateButton(48, ButtonRowY, "_Back", () =>
            {
                _window!.KeyPress -= AgentTypeKeyPress;
                ShowMainMenu();
            }));
    }

    private void AgentTypeKeyPress(View.KeyEventEventArgs args)
    {
        if (args.KeyEvent.Key == Key.N || args.KeyEvent.Key == Key.n || args.KeyEvent.Key == Key.Enter)
        {
            args.Handled = true;
            ActivateButtonByText("_Next");
        }

        if (args.KeyEvent.Key == Key.C || args.KeyEvent.Key == Key.c)
        {
            args.Handled = true;
            ActivateButtonByText("_Back");
        }
    }

    private void ShowVersionStep()
    {
        var versions = GetVersions();
        _versionIndex = Math.Clamp(_versionIndex, 0, Math.Max(versions.Count - 1, 0));
        var listView = CreateListView(versions, _versionIndex, 4, 12);
        listView.SelectedItemChanged += args => _versionIndex = args.Item;

        _window!.KeyPress -= VersionKeyPress;
        _window.KeyPress += VersionKeyPress;

        ReplaceContent(
            "Version",
            new Label(2, 1, "Select the version to install. `latest` uses the head of the main branch."),
            new Label(2, 2, "Available versions:"),
            listView,
            CreateButton(30, ButtonRowY, "_Next", () =>
            {
                _selectedVersion = versions[listView.SelectedItem];
                _versionIndex = listView.SelectedItem;
                _window!.KeyPress -= VersionKeyPress;
                ShowPackagesStep();
            }),
            CreateButton(46, ButtonRowY, "_Back", () =>
            {
                _window!.KeyPress -= VersionKeyPress;
                ShowAgentTypeStep();
            }));
    }

    private void VersionKeyPress(View.KeyEventEventArgs args)
    {
        if (args.KeyEvent.Key == Key.N || args.KeyEvent.Key == Key.n || args.KeyEvent.Key == Key.Enter)
        {
            args.Handled = true;
            ActivateButtonByText("_Next");
        }

        if (args.KeyEvent.Key == Key.B || args.KeyEvent.Key == Key.b || args.KeyEvent.Key == Key.C || args.KeyEvent.Key == Key.c)
        {
            args.Handled = true;
            ActivateButtonByText("_Back");
        }
    }

    private void ShowPackagesStep()
    {
        try
        {
            _availablePackages = GetPackageMetadata(_selectedVersion).Packages;
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Installer", ex.Message, "OK");
            ShowVersionStep();
            return;
        }

        if (_selectedPackages.Count == 0)
        {
            var previous = LoadState()?.Packages;
            _selectedPackages = previous is { Count: > 0 }
                ? new List<string>(previous)
                : _availablePackages.Select(pkg => pkg.Id).ToList();
        }

        var checkBoxes = new List<CheckBox>();
        var views = new List<View>
        {
            new Label(2, 1, "Select the packages to install for the chosen version."),
            new Label(2, 2, "Use space to toggle items. A = Select All, U = Unselect All, N/Enter = Next, B = Back")
        };

        for (var i = 0; i < _availablePackages.Count; i++)
        {
            var package = _availablePackages[i];
            var checkBox = new CheckBox(2, i + 4, package.Id)
            {
                Checked = _selectedPackages.Contains(package.Id)
            };
            checkBoxes.Add(checkBox);
            views.Add(checkBox);
            views.Add(new Label(28, i + 4, package.Description));
        }

        void SyncSelection()
        {
            _selectedPackages = _availablePackages
                .Where((pkg, index) => checkBoxes[index].Checked)
                .Select(pkg => pkg.Id)
                .ToList();
        }

        void SetAll(bool selected)
        {
            foreach (var checkBox in checkBoxes)
            {
                checkBox.Checked = selected;
            }

            SyncSelection();
        }

        views.Add(CreateButton(2, ButtonRowY, "Select _All", () => SetAll(true)));
        views.Add(CreateButton(16, ButtonRowY, "_Unselect All", () => SetAll(false)));
        views.Add(CreateButton(40, ButtonRowY, "_Next", () =>
        {
            SyncSelection();
            ShowReviewStep();
        }));
        views.Add(CreateButton(52, ButtonRowY, "_Back", () =>
        {
            SyncSelection();
            _window!.KeyPress -= PackagesKeyPress;
            ShowVersionStep();
        }));

        _window!.KeyPress -= PackagesKeyPress;
        _window.KeyPress += PackagesKeyPress;
        ReplaceContent("Packages", views.ToArray());
    }

    private void PackagesKeyPress(View.KeyEventEventArgs args)
    {
        if (args.KeyEvent.Key == Key.A || args.KeyEvent.Key == Key.a)
        {
            args.Handled = true;
            ActivateButtonByText("Select _All");
        }

        if (args.KeyEvent.Key == Key.U || args.KeyEvent.Key == Key.u)
        {
            args.Handled = true;
            ActivateButtonByText("_Unselect All");
        }

        if (args.KeyEvent.Key == Key.N || args.KeyEvent.Key == Key.n || args.KeyEvent.Key == Key.Enter)
        {
            args.Handled = true;
            ActivateButtonByText("_Next");
        }

        if (args.KeyEvent.Key == Key.B || args.KeyEvent.Key == Key.b)
        {
            args.Handled = true;
            ActivateButtonByText("_Back");
        }
    }

    private void ShowReviewStep()
    {
        _window!.KeyPress -= PackagesKeyPress;
        _window.KeyPress -= ReviewKeyPress;
        _window.KeyPress += ReviewKeyPress;

        var lines = new List<string>
        {
            $"Agent Type: {_selectedAgentType}",
            $"Version: {_selectedVersion}",
            "Packages:"
        };
        lines.AddRange(_selectedPackages.Select(pkg => $"- {pkg}"));

        ReplaceContent(
            "Review",
            new Label(2, 1, "Review your selections before installation."),
            new TextView
            {
                X = 2,
                Y = 3,
                Width = Dim.Fill() - 4,
                Height = 14,
                ReadOnly = true,
                WordWrap = false,
                Text = string.Join(Environment.NewLine, lines)
            },
            CreateButton(26, ButtonRowY, "_Confirm and Install", ConfirmInstall),
            CreateButton(52, ButtonRowY, "_Back", () =>
            {
                _window!.KeyPress -= ReviewKeyPress;
                ShowPackagesStep();
            }));
    }

    private void ReviewKeyPress(View.KeyEventEventArgs args)
    {
        if (args.KeyEvent.Key == Key.N || args.KeyEvent.Key == Key.n || args.KeyEvent.Key == Key.Enter)
        {
            args.Handled = true;
            ActivateButtonByText("_Confirm and Install");
        }

        if (args.KeyEvent.Key == Key.B || args.KeyEvent.Key == Key.b)
        {
            args.Handled = true;
            ActivateButtonByText("_Back");
        }
    }

    private void ConfirmInstall()
    {
        try
        {
            if (LoadState() is not null)
            {
                RemoveExistingInstallationArtifacts();
            }

            var savedVersion = _selectedVersion == "latest" ? GetLocalVersionLabel() : _selectedVersion;
            SaveState(new InstalledState(savedVersion, _selectedAgentType, new List<string>(_selectedPackages), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            DeployPackages(_selectedVersion, _selectedPackages);

            _window!.KeyPress -= ReviewKeyPress;
            MessageBox.Query("Installer", $"Installation complete.\nVersion: {savedVersion}\nAgent: {_selectedAgentType}", "OK");
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
            RemoveExistingInstallationArtifacts();
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

    private void RemoveExistingInstallationArtifacts()
    {
        var opencodePath = Path.Combine(_workspaceRoot, ".opencode");
        if (Directory.Exists(opencodePath))
        {
            Directory.Delete(opencodePath, true);
        }
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
        using var branchRequest = new HttpRequestMessage(HttpMethod.Get, $"https://github.com/{RepoOwner}/{RepoName}/archive/refs/heads/{branch}.zip");
        branchRequest.Headers.Add("User-Agent", "AgenticSkillsInstaller");
        var response = _httpClient.Send(branchRequest);

        if (!response.IsSuccessStatusCode && branch != "main")
        {
            response.Dispose();
            using var tagRequest = new HttpRequestMessage(HttpMethod.Get, $"https://github.com/{RepoOwner}/{RepoName}/archive/refs/tags/{branch}.zip");
            tagRequest.Headers.Add("User-Agent", "AgenticSkillsInstaller");
            var tagResponse = _httpClient.Send(tagRequest);
            tagResponse.EnsureSuccessStatusCode();
            File.WriteAllBytes(zipPath, tagResponse.Content.ReadAsByteArrayAsync().Result);
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

    private static ListView CreateListView(IList<string> items, int selectedItem, int y, int height)
    {
        return new ListView(new ArrayList(items.ToArray()))
        {
            X = 2,
            Y = y,
            Width = Dim.Fill() - 4,
            Height = height,
            SelectedItem = selectedItem
        };
    }

    private static Button CreateButton(int x, int y, string text, Action onClick)
    {
        var button = new Button(text)
        {
            X = x,
            Y = y
        };
        button.Clicked += onClick;
        return button;
    }

    private void ActivateButtonByText(string buttonText)
    {
        if (_contentFrame is null)
        {
            return;
        }

        var button = _contentFrame.Subviews.OfType<Button>().FirstOrDefault(b => b.Text.ToString() == buttonText);
        button?.OnClicked();
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
