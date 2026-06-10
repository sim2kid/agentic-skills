using System.Collections;
using System.Text;
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
    private const int ShellHeight = 32;
    private const int ButtonRowY = 21;
    private const int PackagePageSize = 8;

    private readonly HttpClient _httpClient = new();
    private readonly string _workspaceRoot = Environment.CurrentDirectory;

    private Window? _window;
    private FrameView? _infoFrame;
    private FrameView? _contentFrame;
    private string _selectedAgentType = DefaultAgentType;
    private string _selectedVersion = "latest";
    private List<string> _selectedPackages = new();
    private List<PackageDefinition> _availablePackages = new();
    private readonly Dictionary<string, HashSet<string>> _forcedBy = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _manuallySelectedPackages = new(StringComparer.OrdinalIgnoreCase);
    private int _agentTypeIndex;
    private int _versionIndex;
    private int _packagePageIndex;
    private string? _focusedPackageId;

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
            new Label(28, 5, "Press Ctrl+C at any time to exit."),
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
        if (_agentTypeIndex < 0 || _agentTypeIndex >= options.Count)
        {
            _agentTypeIndex = 0;
        }

        var listView = CreateListView(options, _agentTypeIndex, 4, 12);
        listView.SelectedItem = _agentTypeIndex;
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

        listView.SetFocus();
    }

    private void AgentTypeKeyPress(View.KeyEventEventArgs args)
    {
        if (args.KeyEvent.Key == Key.N || args.KeyEvent.Key == Key.n || args.KeyEvent.Key == Key.Enter)
        {
            args.Handled = true;
            AdvanceAgentType();
        }

        if (args.KeyEvent.Key == Key.C || args.KeyEvent.Key == Key.c || args.KeyEvent.Key == Key.B || args.KeyEvent.Key == Key.b)
        {
            args.Handled = true;
            ShowMainMenu();
        }
    }

    private void AdvanceAgentType()
    {
        var options = new List<string> { DefaultAgentType };
        _selectedAgentType = options[Math.Clamp(_agentTypeIndex, 0, options.Count - 1)];
        ShowVersionStep();
    }

    private void ShowVersionStep()
    {
        var versions = GetVersions();
        _versionIndex = Math.Clamp(_versionIndex, 0, Math.Max(versions.Count - 1, 0));
        var listView = CreateListView(versions, _versionIndex, 4, 12);
        listView.SelectedItem = _versionIndex;
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

        listView.SetFocus();
    }

    private void VersionKeyPress(View.KeyEventEventArgs args)
    {
        if (args.KeyEvent.Key == Key.N || args.KeyEvent.Key == Key.n || args.KeyEvent.Key == Key.Enter)
        {
            args.Handled = true;
            AdvanceVersion();
        }

        if (args.KeyEvent.Key == Key.B || args.KeyEvent.Key == Key.b || args.KeyEvent.Key == Key.C || args.KeyEvent.Key == Key.c)
        {
            args.Handled = true;
            ShowAgentTypeStep();
        }
    }

    private void AdvanceVersion()
    {
        var versions = GetVersions();
        if (versions.Count == 0)
        {
            versions = new List<string> { "latest" };
        }

        _selectedVersion = versions[Math.Clamp(_versionIndex, 0, versions.Count - 1)];
        ShowPackagesStep();
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

        _manuallySelectedPackages.Clear();
        foreach (var packageId in _selectedPackages)
        {
            _manuallySelectedPackages.Add(packageId);
        }

        NormalizeForcedDependencies();
        RenderPackagesPage();
    }

    private void RenderPackagesPage()
    {
        _window!.KeyPress -= PackagesKeyPress;
        _window.KeyPress += PackagesKeyPress;

        var totalPages = Math.Max(1, (int)Math.Ceiling(_availablePackages.Count / (double)PackagePageSize));
        _packagePageIndex = Math.Clamp(_packagePageIndex, 0, totalPages - 1);
        var startIndex = _packagePageIndex * PackagePageSize;
        var pagePackages = _availablePackages.Skip(startIndex).Take(PackagePageSize).ToList();
        var restoreFocusId = _focusedPackageId;

        var descriptionView = new TextView
        {
            X = 2,
            Y = 15,
            Width = Dim.Fill() - 4,
            Height = 6,
            ReadOnly = true,
            WordWrap = true,
            CanFocus = false,
            Text = string.Empty
        };

        var packageRows = new List<PackageRow>();
        var views = new List<View>
        {
            new Label(2, 1, "Select packages to install. Space or Enter toggles the selected package."),
            CreateButton(2, 3, "Select _All", () =>
            {
                SelectAllPackages();
                RenderPackagesPage();
            }),
            CreateButton(16, 3, "_Unselect All", () =>
            {
                UnselectAllPackages();
                RenderPackagesPage();
            }),
            new Label(34, 3, $"Page {_packagePageIndex + 1} of {totalPages}")
        };

        for (var index = 0; index < pagePackages.Count; index++)
        {
            var package = pagePackages[index];
            var row = CreatePackageRow(package, startIndex + index, 5 + index, descriptionView);
            packageRows.Add(row);
            views.Add(row.Toggle);
            views.Add(row.Status);
        }

        views.Add(new Label(2, 13, "Package details:"));
        views.Add(descriptionView);

        if (totalPages > 1)
        {
            views.Add(CreateButton(2, ButtonRowY, "_Previous Page", () =>
            {
                _packagePageIndex = Math.Max(0, _packagePageIndex - 1);
                RenderPackagesPage();
            }));

            views.Add(CreateButton(20, ButtonRowY, "_Next Page", () =>
            {
                _packagePageIndex = Math.Min(totalPages - 1, _packagePageIndex + 1);
                RenderPackagesPage();
            }));
        }

        views.Add(CreateButton(40, ButtonRowY, "_Next", ShowReviewStep));
        views.Add(CreateButton(52, ButtonRowY, "_Back", () =>
        {
            _window!.KeyPress -= PackagesKeyPress;
            ShowVersionStep();
        }));

        ReplaceContent("Packages", views.ToArray());

        if (packageRows.Count > 0)
        {
            var focusIndex = 0;
            if (!string.IsNullOrWhiteSpace(restoreFocusId))
            {
                var matchedIndex = pagePackages.FindIndex(pkg => string.Equals(pkg.Id, restoreFocusId, StringComparison.OrdinalIgnoreCase));
                if (matchedIndex >= 0)
                {
                    focusIndex = matchedIndex;
                }
            }

            _focusedPackageId = pagePackages[focusIndex].Id;
            UpdateDescription(descriptionView, pagePackages[focusIndex]);
            packageRows[focusIndex].Toggle.SetFocus();
        }
    }

    private PackageRow CreatePackageRow(PackageDefinition package, int absoluteIndex, int y, TextView descriptionView)
    {
        var toggle = new CheckBox(2, y, package.Id)
        {
            Checked = _selectedPackages.Contains(package.Id)
        };
        var status = new Label(28, y, GetPackageStatusText(package.Id));

        toggle.Enter += _ => UpdateDescription(descriptionView, package);
        toggle.Leave += _ => UpdateDescription(descriptionView, package);
        toggle.KeyPress += args =>
        {
            if (args.KeyEvent.Key == Key.Enter || args.KeyEvent.Key == Key.Space)
            {
                args.Handled = true;
                _focusedPackageId = package.Id;
                TogglePackage(package.Id);
                RenderPackagesPage();
            }
        };
        toggle.MouseClick += _ =>
        {
            _focusedPackageId = package.Id;
            UpdateDescription(descriptionView, package);
            TogglePackage(package.Id);
            RenderPackagesPage();
        };

        return new PackageRow(toggle, status, absoluteIndex);
    }

    private void UpdateDescription(TextView descriptionView, PackageDefinition package)
    {
        var builder = new StringBuilder();
        builder.AppendLine(package.Description);
        if (package.Dependencies.Count > 0)
        {
            builder.AppendLine();
            builder.AppendLine($"Dependencies: {string.Join(", ", package.Dependencies)}");
        }

        var forcedBy = GetForcedByText(package.Id);
        if (!string.IsNullOrWhiteSpace(forcedBy))
        {
            builder.AppendLine();
            builder.AppendLine(forcedBy);
        }

        descriptionView.Text = builder.ToString();
    }

    private void PackagesKeyPress(View.KeyEventEventArgs args)
    {
        if (args.KeyEvent.Key == Key.A || args.KeyEvent.Key == Key.a)
        {
            args.Handled = true;
            SelectAllPackages();
            RenderPackagesPage();
        }

        if (args.KeyEvent.Key == Key.U || args.KeyEvent.Key == Key.u)
        {
            args.Handled = true;
            UnselectAllPackages();
            RenderPackagesPage();
        }

        if (args.KeyEvent.Key == Key.N || args.KeyEvent.Key == Key.n)
        {
            args.Handled = true;
            ShowReviewStep();
        }

        if (args.KeyEvent.Key == Key.B || args.KeyEvent.Key == Key.b)
        {
            args.Handled = true;
            ActivateButtonByText("_Back");
        }

        if (args.KeyEvent.Key == Key.PageDown)
        {
            args.Handled = true;
            ActivateButtonByText("_Next Page");
        }

        if (args.KeyEvent.Key == Key.PageUp)
        {
            args.Handled = true;
            ActivateButtonByText("_Previous Page");
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
        lines.AddRange(_selectedPackages.Select(pkg => $"- {pkg}{GetForcedSuffix(pkg)}"));

        ReplaceContent(
            "Review",
            new Label(2, 1, "Review your selections before installation."),
            new TextView
            {
                X = 2,
                Y = 3,
                Width = Dim.Fill() - 4,
                Height = 16,
                ReadOnly = true,
                WordWrap = false,
                CanFocus = false,
                Text = string.Join(Environment.NewLine, lines)
            },
            CreateButton(26, ButtonRowY, "_Confirm and Install", ConfirmInstall),
            CreateButton(52, ButtonRowY, "_Back", () =>
            {
                _window!.KeyPress -= ReviewKeyPress;
                RenderPackagesPage();
            }));
    }

    private void ReviewKeyPress(View.KeyEventEventArgs args)
    {
        if (args.KeyEvent.Key == Key.C || args.KeyEvent.Key == Key.c || args.KeyEvent.Key == Key.N || args.KeyEvent.Key == Key.n || args.KeyEvent.Key == Key.Enter)
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
            var close = MessageBox.Query("Installer", $"Installation complete.\nVersion: {savedVersion}\nAgent: {_selectedAgentType}", "_Close");
            if (close == 0)
            {
                Application.RequestStop();
            }
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Installer", ex.Message, "OK");
        }
    }

    private void TogglePackage(string packageId)
    {
        if (_selectedPackages.Contains(packageId))
        {
            if (_manuallySelectedPackages.Contains(packageId))
            {
                _manuallySelectedPackages.Remove(packageId);
            }

            if (_forcedBy.TryGetValue(packageId, out var forcedBy) && forcedBy.Count > 0)
            {
                MessageBox.ErrorQuery("Dependency Required", $"`{packageId}` is required by: {string.Join(", ", forcedBy.OrderBy(x => x))}", "OK");
                return;
            }

            _selectedPackages.Remove(packageId);
            RemoveDependencyForcers(packageId);
            return;
        }

        _manuallySelectedPackages.Add(packageId);
        AddPackageWithDependencies(packageId, packageId);
    }

    private void AddPackageWithDependencies(string packageId, string requestedBy)
    {
        if (!_selectedPackages.Contains(packageId))
        {
            _selectedPackages.Add(packageId);
        }

        var package = _availablePackages.FirstOrDefault(pkg => string.Equals(pkg.Id, packageId, StringComparison.OrdinalIgnoreCase));
        if (package is null)
        {
            return;
        }

        foreach (var dependency in package.Dependencies)
        {
            if (!_forcedBy.TryGetValue(dependency, out var forcedBy))
            {
                forcedBy = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                _forcedBy[dependency] = forcedBy;
            }

            forcedBy.Add(packageId == requestedBy ? packageId : requestedBy);
            AddPackageWithDependencies(dependency, packageId == requestedBy ? packageId : requestedBy);
        }
    }

    private void RemoveDependencyForcers(string packageId)
    {
        foreach (var entry in _forcedBy.Values)
        {
            entry.Remove(packageId);
        }

        var autoSelected = _forcedBy
            .Where(pair => pair.Value.Count == 0)
            .Select(pair => pair.Key)
            .ToList();

        foreach (var dependency in autoSelected)
        {
            _forcedBy.Remove(dependency);
            if (_selectedPackages.Contains(dependency) && !IsRequiredBySelectedPackage(dependency) && !_manuallySelectedPackages.Contains(dependency))
            {
                _selectedPackages.Remove(dependency);
                RemoveDependencyForcers(dependency);
            }
        }
    }

    private bool IsRequiredBySelectedPackage(string dependencyId)
    {
        return _availablePackages
            .Where(pkg => _selectedPackages.Contains(pkg.Id))
            .Any(pkg => pkg.Dependencies.Contains(dependencyId, StringComparer.OrdinalIgnoreCase));
    }

    private void NormalizeForcedDependencies()
    {
        _forcedBy.Clear();

        var selectedSet = new HashSet<string>(_selectedPackages, StringComparer.OrdinalIgnoreCase);
        var dependencyOwners = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        foreach (var selected in _selectedPackages.ToList())
        {
            CollectDependencies(selected, selected, selectedSet, dependencyOwners, new HashSet<string>(StringComparer.OrdinalIgnoreCase));
        }

        _selectedPackages = _availablePackages
            .Where(pkg => selectedSet.Contains(pkg.Id))
            .Select(pkg => pkg.Id)
            .ToList();

        foreach (var pair in dependencyOwners)
        {
            _forcedBy[pair.Key] = pair.Value;
        }
    }

    private void CollectDependencies(string rootPackageId, string currentPackageId, HashSet<string> selectedSet, Dictionary<string, HashSet<string>> dependencyOwners, HashSet<string> visited)
    {
        if (!visited.Add(currentPackageId))
        {
            return;
        }

        var package = _availablePackages.FirstOrDefault(pkg => string.Equals(pkg.Id, currentPackageId, StringComparison.OrdinalIgnoreCase));
        if (package is null)
        {
            return;
        }

        foreach (var dependency in package.Dependencies)
        {
            selectedSet.Add(dependency);

            if (!dependencyOwners.TryGetValue(dependency, out var owners))
            {
                owners = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                dependencyOwners[dependency] = owners;
            }

            owners.Add(rootPackageId);

            CollectDependencies(rootPackageId, dependency, selectedSet, dependencyOwners, visited);
        }
    }

    private void SelectAllPackages()
    {
        _selectedPackages = _availablePackages.Select(pkg => pkg.Id).ToList();
        NormalizeForcedDependencies();
    }

    private void UnselectAllPackages()
    {
        _selectedPackages.Clear();
        _forcedBy.Clear();
    }

    private string GetPackageStatusText(string packageId)
    {
        if (_forcedBy.TryGetValue(packageId, out var forcedBy) && forcedBy.Count > 0)
        {
            return $"[!] Required by {string.Join(", ", forcedBy.OrderBy(x => x))}";
        }

        if (_selectedPackages.Contains(packageId) && _manuallySelectedPackages.Contains(packageId))
        {
            return "[X] Selected";
        }

        if (_selectedPackages.Contains(packageId))
        {
            return "[X] Selected by dependency";
        }

        return "[ ] Optional";
    }

    private string GetForcedByText(string packageId)
    {
        if (_forcedBy.TryGetValue(packageId, out var forcedBy) && forcedBy.Count > 0)
        {
            return $"Required by: {string.Join(", ", forcedBy.OrderBy(x => x))}";
        }

        return string.Empty;
    }

    private string GetForcedSuffix(string packageId)
    {
        return _forcedBy.TryGetValue(packageId, out var forcedBy) && forcedBy.Count > 0
            ? $" [required by {string.Join(", ", forcedBy.OrderBy(x => x))}]"
            : string.Empty;
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
            return JsonSerializer.Deserialize<PackageMetadata>(File.ReadAllText(localPath), PackageJsonOptions())
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
            return JsonSerializer.Deserialize<PackageMetadata>(fallbackResponse.Content.ReadAsStringAsync().Result, PackageJsonOptions())
                ?? throw new InvalidOperationException("Failed to parse packages.json from main.");
        }

        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<PackageMetadata>(response.Content.ReadAsStringAsync().Result, PackageJsonOptions())
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

    private static JsonSerializerOptions PackageJsonOptions() => new()
    {
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true
    };
}

internal sealed record PackageRow(CheckBox Toggle, Label Status, int AbsoluteIndex);
internal sealed record GitTag(string Name);
internal sealed record GitCommit(string Sha);
internal sealed record InstalledState(string Version, string AgentType, List<string> Packages, string Timestamp);
internal sealed record PackageMetadata(List<PackageDefinition> Packages);
internal sealed record PackageDefinition(string Id, string Description, List<string> Dependencies);
