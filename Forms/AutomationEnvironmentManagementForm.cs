using RpaAlmClient.Models;
using RpaAlmClient.Services;

namespace RpaAlmClient.Forms;

public partial class AutomationEnvironmentManagementForm : Form
{
    private readonly AutomationEnvironmentApiClient _automationEnvironmentApiClient;
    private DataGridView dgvAutomationEnvironments = null!;
    private TextBox txtAutomationID = null!;
    private TextBox txtAppID = null!;
    private TextBox txtEnvironmentTypeID = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblAutomationID = null!;
    private Label lblAppID = null!;
    private Label lblEnvironmentTypeID = null!;
    private int? _selectedAutomationEnvironmentId;

    public AutomationEnvironmentManagementForm()
    {
        _automationEnvironmentApiClient = new AutomationEnvironmentApiClient();
        InitializeComponents();
        _ = LoadAutomationEnvironmentsAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - Automation Environment Management";
        this.Size = new Size(950, 500);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvAutomationEnvironments = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 400),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvAutomationEnvironments.SelectionChanged += DgvAutomationEnvironments_SelectionChanged;

        // Labels and TextBoxes
        int labelX = 580;
        int textBoxX = 750;
        int startY = 30;
        int yIncrement = 40;
        int currentY = startY;

        lblAutomationID = new Label { Text = "AutomationID:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtAutomationID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(170, 23) };
        currentY += yIncrement;

        lblAppID = new Label { Text = "AppID:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtAppID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(170, 23), MaxLength = 50 };
        currentY += yIncrement;

        lblEnvironmentTypeID = new Label { Text = "EnvironmentTypeID:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtEnvironmentTypeID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(170, 23) };
        currentY += yIncrement;

        // Buttons
        btnAdd = new Button { Text = "Add New", Location = new Point(580, currentY + 20), Size = new Size(100, 30) };
        btnUpdate = new Button { Text = "Update", Location = new Point(690, currentY + 20), Size = new Size(100, 30), Enabled = false };
        btnDelete = new Button { Text = "Delete", Location = new Point(800, currentY + 20), Size = new Size(70, 30), Enabled = false };
        btnRefresh = new Button { Text = "Refresh", Location = new Point(580, currentY + 60), Size = new Size(100, 30) };

        btnAdd.Click += async (s, e) => await AddAutomationEnvironmentAsync();
        btnUpdate.Click += async (s, e) => await UpdateAutomationEnvironmentAsync();
        btnDelete.Click += async (s, e) => await DeleteAutomationEnvironmentAsync();
        btnRefresh.Click += async (s, e) => await LoadAutomationEnvironmentsAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvAutomationEnvironments,
            lblAutomationID, lblAppID, lblEnvironmentTypeID,
            txtAutomationID, txtAppID, txtEnvironmentTypeID,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadAutomationEnvironmentsAsync()
    {
        try
        {
            var automationEnvironments = await _automationEnvironmentApiClient.GetAllAsync();
            dgvAutomationEnvironments.DataSource = automationEnvironments;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading automation environments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvAutomationEnvironments_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvAutomationEnvironments.SelectedRows.Count > 0)
        {
            var row = dgvAutomationEnvironments.SelectedRows[0];
            var automationEnvironment = row.DataBoundItem as AutomationEnvironmentDto;

            if (automationEnvironment != null)
            {
                _selectedAutomationEnvironmentId = automationEnvironment.Id;
                txtAutomationID.Text = automationEnvironment.AutomationID?.ToString() ?? string.Empty;
                txtAppID.Text = automationEnvironment.AppID ?? string.Empty;
                txtEnvironmentTypeID.Text = automationEnvironment.EnvironmentTypeID?.ToString() ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddAutomationEnvironmentAsync()
    {
        try
        {
            var request = new AutomationEnvironmentCreateRequest
            {
                AutomationID = ParseNullableInt(txtAutomationID.Text),
                AppID = string.IsNullOrWhiteSpace(txtAppID.Text) ? null : txtAppID.Text.Trim(),
                EnvironmentTypeID = ParseNullableInt(txtEnvironmentTypeID.Text)
            };

            await _automationEnvironmentApiClient.CreateAsync(request);
            MessageBox.Show("Automation Environment created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadAutomationEnvironmentsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating automation environment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateAutomationEnvironmentAsync()
    {
        try
        {
            if (_selectedAutomationEnvironmentId == null)
            {
                MessageBox.Show("Please select an automation environment to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new AutomationEnvironmentUpdateRequest
            {
                AutomationID = ParseNullableInt(txtAutomationID.Text),
                AppID = string.IsNullOrWhiteSpace(txtAppID.Text) ? null : txtAppID.Text.Trim(),
                EnvironmentTypeID = ParseNullableInt(txtEnvironmentTypeID.Text)
            };

            await _automationEnvironmentApiClient.UpdateAsync(_selectedAutomationEnvironmentId.Value, request);
            MessageBox.Show("Automation Environment updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadAutomationEnvironmentsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating automation environment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteAutomationEnvironmentAsync()
    {
        try
        {
            if (_selectedAutomationEnvironmentId == null)
            {
                MessageBox.Show("Please select an automation environment to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this automation environment?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                await _automationEnvironmentApiClient.DeleteAsync(_selectedAutomationEnvironmentId.Value);
                MessageBox.Show("Automation Environment deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadAutomationEnvironmentsAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting automation environment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedAutomationEnvironmentId = null;
        txtAutomationID.Text = string.Empty;
        txtAppID.Text = string.Empty;
        txtEnvironmentTypeID.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnDelete.Enabled = false;
    }

    private int? ParseNullableInt(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (int.TryParse(value.Trim(), out int result))
            return result;

        return null;
    }
}
