using RpaAlmClient.Models;
using RpaAlmClient.Services;

namespace RpaAlmClient.Forms;

public partial class AutomationManagementForm : Form
{
    private readonly AutomationApiClient _automationApiClient;
    private DataGridView dgvAutomations = null!;
    private TextBox txtName = null!;
    private TextBox txtSegmentID = null!;
    private TextBox txtRegionID = null!;
    private TextBox txtFunctionID = null!;
    private TextBox txtStatusID = null!;
    private TextBox txtBtoWWID = null!;
    private TextBox txtBoWWID = null!;
    private TextBox txtFcWWID = null!;
    private TextBox txtBuildZcode = null!;
    private TextBox txtBuildCostCenter = null!;
    private TextBox txtSseWWID = null!;
    private TextBox txtLseWWID = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblName = null!;
    private Label lblSegmentID = null!;
    private Label lblRegionID = null!;
    private Label lblFunctionID = null!;
    private Label lblStatusID = null!;
    private Label lblBtoWWID = null!;
    private Label lblBoWWID = null!;
    private Label lblFcWWID = null!;
    private Label lblBuildZcode = null!;
    private Label lblBuildCostCenter = null!;
    private Label lblSseWWID = null!;
    private Label lblLseWWID = null!;
    private int? _selectedAutomationId;

    public AutomationManagementForm()
    {
        _automationApiClient = new AutomationApiClient();
        InitializeComponents();
        _ = LoadAutomationsAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - Automation Management";
        this.Size = new Size(1100, 700);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvAutomations = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(640, 600),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvAutomations.SelectionChanged += DgvAutomations_SelectionChanged;

        // Labels and TextBoxes
        int labelX = 680;
        int textBoxX = 850;
        int startY = 30;
        int yIncrement = 40;
        int currentY = startY;

        lblName = new Label { Text = "Name:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtName = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23), MaxLength = 255 };
        currentY += yIncrement;

        lblSegmentID = new Label { Text = "SegmentID:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtSegmentID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23) };
        currentY += yIncrement;

        lblRegionID = new Label { Text = "RegionID:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtRegionID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23) };
        currentY += yIncrement;

        lblFunctionID = new Label { Text = "FunctionID:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtFunctionID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23) };
        currentY += yIncrement;

        lblStatusID = new Label { Text = "StatusID:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtStatusID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23) };
        currentY += yIncrement;

        lblBtoWWID = new Label { Text = "BtoWWID:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtBtoWWID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23), MaxLength = 9 };
        currentY += yIncrement;

        lblBoWWID = new Label { Text = "BoWWID:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtBoWWID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23), MaxLength = 9 };
        currentY += yIncrement;

        lblFcWWID = new Label { Text = "FcWWID:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtFcWWID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23), MaxLength = 9 };
        currentY += yIncrement;

        lblBuildZcode = new Label { Text = "BuildZcode:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtBuildZcode = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23), MaxLength = 10 };
        currentY += yIncrement;

        lblBuildCostCenter = new Label { Text = "BuildCostCenter:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtBuildCostCenter = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23), MaxLength = 10 };
        currentY += yIncrement;

        lblSseWWID = new Label { Text = "SseWWID:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtSseWWID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23), MaxLength = 9 };
        currentY += yIncrement;

        lblLseWWID = new Label { Text = "LseWWID:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtLseWWID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23), MaxLength = 9 };
        currentY += yIncrement;

        // Buttons
        btnAdd = new Button { Text = "Add New", Location = new Point(680, currentY + 20), Size = new Size(100, 30) };
        btnUpdate = new Button { Text = "Update", Location = new Point(790, currentY + 20), Size = new Size(100, 30), Enabled = false };
        btnDelete = new Button { Text = "Delete", Location = new Point(900, currentY + 20), Size = new Size(80, 30), Enabled = false };
        btnRefresh = new Button { Text = "Refresh", Location = new Point(990, currentY + 20), Size = new Size(80, 30) };

        btnAdd.Click += async (s, e) => await AddAutomationAsync();
        btnUpdate.Click += async (s, e) => await UpdateAutomationAsync();
        btnDelete.Click += async (s, e) => await DeleteAutomationAsync();
        btnRefresh.Click += async (s, e) => await LoadAutomationsAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvAutomations,
            lblName, lblSegmentID, lblRegionID, lblFunctionID, lblStatusID,
            lblBtoWWID, lblBoWWID, lblFcWWID, lblBuildZcode, lblBuildCostCenter,
            lblSseWWID, lblLseWWID,
            txtName, txtSegmentID, txtRegionID, txtFunctionID, txtStatusID,
            txtBtoWWID, txtBoWWID, txtFcWWID, txtBuildZcode, txtBuildCostCenter,
            txtSseWWID, txtLseWWID,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadAutomationsAsync()
    {
        try
        {
            var automations = await _automationApiClient.GetAllAsync();
            dgvAutomations.DataSource = automations;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading automations: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvAutomations_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvAutomations.SelectedRows.Count > 0)
        {
            var row = dgvAutomations.SelectedRows[0];
            var automation = row.DataBoundItem as AutomationDto;

            if (automation != null)
            {
                _selectedAutomationId = automation.Id;
                txtName.Text = automation.Name ?? string.Empty;
                txtSegmentID.Text = automation.SegmentID?.ToString() ?? string.Empty;
                txtRegionID.Text = automation.RegionID?.ToString() ?? string.Empty;
                txtFunctionID.Text = automation.FunctionID?.ToString() ?? string.Empty;
                txtStatusID.Text = automation.StatusID?.ToString() ?? string.Empty;
                txtBtoWWID.Text = automation.BtoWWID ?? string.Empty;
                txtBoWWID.Text = automation.BoWWID ?? string.Empty;
                txtFcWWID.Text = automation.FcWWID ?? string.Empty;
                txtBuildZcode.Text = automation.BuildZcode ?? string.Empty;
                txtBuildCostCenter.Text = automation.BuildCostCenter ?? string.Empty;
                txtSseWWID.Text = automation.SseWWID ?? string.Empty;
                txtLseWWID.Text = automation.LseWWID ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddAutomationAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new AutomationCreateRequest
            {
                Name = txtName.Text.Trim(),
                SegmentID = ParseNullableInt(txtSegmentID.Text),
                RegionID = ParseNullableInt(txtRegionID.Text),
                FunctionID = ParseNullableInt(txtFunctionID.Text),
                StatusID = ParseNullableInt(txtStatusID.Text),
                BtoWWID = string.IsNullOrWhiteSpace(txtBtoWWID.Text) ? null : txtBtoWWID.Text.Trim(),
                BoWWID = string.IsNullOrWhiteSpace(txtBoWWID.Text) ? null : txtBoWWID.Text.Trim(),
                FcWWID = string.IsNullOrWhiteSpace(txtFcWWID.Text) ? null : txtFcWWID.Text.Trim(),
                BuildZcode = string.IsNullOrWhiteSpace(txtBuildZcode.Text) ? null : txtBuildZcode.Text.Trim(),
                BuildCostCenter = string.IsNullOrWhiteSpace(txtBuildCostCenter.Text) ? null : txtBuildCostCenter.Text.Trim(),
                SseWWID = string.IsNullOrWhiteSpace(txtSseWWID.Text) ? null : txtSseWWID.Text.Trim(),
                LseWWID = string.IsNullOrWhiteSpace(txtLseWWID.Text) ? null : txtLseWWID.Text.Trim()
            };

            await _automationApiClient.CreateAsync(request);
            MessageBox.Show("Automation created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadAutomationsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating automation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateAutomationAsync()
    {
        try
        {
            if (_selectedAutomationId == null)
            {
                MessageBox.Show("Please select an automation to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new AutomationUpdateRequest
            {
                Name = txtName.Text.Trim(),
                SegmentID = ParseNullableInt(txtSegmentID.Text),
                RegionID = ParseNullableInt(txtRegionID.Text),
                FunctionID = ParseNullableInt(txtFunctionID.Text),
                StatusID = ParseNullableInt(txtStatusID.Text),
                BtoWWID = string.IsNullOrWhiteSpace(txtBtoWWID.Text) ? null : txtBtoWWID.Text.Trim(),
                BoWWID = string.IsNullOrWhiteSpace(txtBoWWID.Text) ? null : txtBoWWID.Text.Trim(),
                FcWWID = string.IsNullOrWhiteSpace(txtFcWWID.Text) ? null : txtFcWWID.Text.Trim(),
                BuildZcode = string.IsNullOrWhiteSpace(txtBuildZcode.Text) ? null : txtBuildZcode.Text.Trim(),
                BuildCostCenter = string.IsNullOrWhiteSpace(txtBuildCostCenter.Text) ? null : txtBuildCostCenter.Text.Trim(),
                SseWWID = string.IsNullOrWhiteSpace(txtSseWWID.Text) ? null : txtSseWWID.Text.Trim(),
                LseWWID = string.IsNullOrWhiteSpace(txtLseWWID.Text) ? null : txtLseWWID.Text.Trim()
            };

            await _automationApiClient.UpdateAsync(_selectedAutomationId.Value, request);
            MessageBox.Show("Automation updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadAutomationsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating automation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteAutomationAsync()
    {
        try
        {
            if (_selectedAutomationId == null)
            {
                MessageBox.Show("Please select an automation to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this automation?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                await _automationApiClient.DeleteAsync(_selectedAutomationId.Value);
                MessageBox.Show("Automation deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadAutomationsAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting automation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedAutomationId = null;
        txtName.Text = string.Empty;
        txtSegmentID.Text = string.Empty;
        txtRegionID.Text = string.Empty;
        txtFunctionID.Text = string.Empty;
        txtStatusID.Text = string.Empty;
        txtBtoWWID.Text = string.Empty;
        txtBoWWID.Text = string.Empty;
        txtFcWWID.Text = string.Empty;
        txtBuildZcode.Text = string.Empty;
        txtBuildCostCenter.Text = string.Empty;
        txtSseWWID.Text = string.Empty;
        txtLseWWID.Text = string.Empty;
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
