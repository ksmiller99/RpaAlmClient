using RpaAlmClient.Models;
using RpaAlmClient.Services;

namespace RpaAlmClient.Forms;

public partial class SlaMasterManagementForm : Form
{
    private readonly SlaMasterApiClient _slaMasterApiClient;
    private DataGridView dgvSlaMasters = null!;
    private TextBox txtAutomationID = null!;
    private TextBox txtComplexityID = null!;
    private TextBox txtMedalID = null!;
    private TextBox txtZcode = null!;
    private TextBox txtCostCenter = null!;
    private TextBox txtStartDate = null!;
    private TextBox txtEndDate = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblAutomationID = null!;
    private Label lblComplexityID = null!;
    private Label lblMedalID = null!;
    private Label lblZcode = null!;
    private Label lblCostCenter = null!;
    private Label lblStartDate = null!;
    private Label lblEndDate = null!;
    private int? _selectedSlaMasterId;

    public SlaMasterManagementForm()
    {
        _slaMasterApiClient = new SlaMasterApiClient();
        InitializeComponents();
        _ = LoadSlaMastersAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - SLA Master Management";
        this.Size = new Size(1000, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvSlaMasters = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 500),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvSlaMasters.SelectionChanged += DgvSlaMasters_SelectionChanged;

        // Labels and TextBoxes
        int labelX = 580;
        int textBoxX = 710;
        int startY = 30;
        int yIncrement = 40;
        int currentY = startY;

        lblAutomationID = new Label { Text = "AutomationID:", Location = new Point(labelX, currentY), Size = new Size(120, 23) };
        txtAutomationID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(250, 23) };
        currentY += yIncrement;

        lblComplexityID = new Label { Text = "ComplexityID:", Location = new Point(labelX, currentY), Size = new Size(120, 23) };
        txtComplexityID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(250, 23) };
        currentY += yIncrement;

        lblMedalID = new Label { Text = "MedalID:", Location = new Point(labelX, currentY), Size = new Size(120, 23) };
        txtMedalID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(250, 23) };
        currentY += yIncrement;

        lblZcode = new Label { Text = "Zcode:", Location = new Point(labelX, currentY), Size = new Size(120, 23) };
        txtZcode = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(250, 23), MaxLength = 10 };
        currentY += yIncrement;

        lblCostCenter = new Label { Text = "CostCenter:", Location = new Point(labelX, currentY), Size = new Size(120, 23) };
        txtCostCenter = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(250, 23), MaxLength = 10 };
        currentY += yIncrement;

        lblStartDate = new Label { Text = "StartDate:", Location = new Point(labelX, currentY), Size = new Size(120, 23) };
        txtStartDate = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(250, 23) };
        currentY += yIncrement;

        lblEndDate = new Label { Text = "EndDate:", Location = new Point(labelX, currentY), Size = new Size(120, 23) };
        txtEndDate = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(250, 23) };
        currentY += yIncrement;

        // Buttons
        btnAdd = new Button { Text = "Add New", Location = new Point(580, currentY + 20), Size = new Size(100, 30) };
        btnUpdate = new Button { Text = "Update", Location = new Point(690, currentY + 20), Size = new Size(100, 30), Enabled = false };
        btnDelete = new Button { Text = "Delete", Location = new Point(800, currentY + 20), Size = new Size(80, 30), Enabled = false };
        btnRefresh = new Button { Text = "Refresh", Location = new Point(890, currentY + 20), Size = new Size(80, 30) };

        btnAdd.Click += async (s, e) => await AddSlaMasterAsync();
        btnUpdate.Click += async (s, e) => await UpdateSlaMasterAsync();
        btnDelete.Click += async (s, e) => await DeleteSlaMasterAsync();
        btnRefresh.Click += async (s, e) => await LoadSlaMastersAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvSlaMasters,
            lblAutomationID, lblComplexityID, lblMedalID, lblZcode, lblCostCenter, lblStartDate, lblEndDate,
            txtAutomationID, txtComplexityID, txtMedalID, txtZcode, txtCostCenter, txtStartDate, txtEndDate,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadSlaMastersAsync()
    {
        try
        {
            var slaMasters = await _slaMasterApiClient.GetAllAsync();
            dgvSlaMasters.DataSource = slaMasters;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading SLA masters: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvSlaMasters_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvSlaMasters.SelectedRows.Count > 0)
        {
            var row = dgvSlaMasters.SelectedRows[0];
            var slaMaster = row.DataBoundItem as SlaMasterDto;

            if (slaMaster != null)
            {
                _selectedSlaMasterId = slaMaster.Id;
                txtAutomationID.Text = slaMaster.AutomationID?.ToString() ?? string.Empty;
                txtComplexityID.Text = slaMaster.ComplexityID?.ToString() ?? string.Empty;
                txtMedalID.Text = slaMaster.MedalID?.ToString() ?? string.Empty;
                txtZcode.Text = slaMaster.Zcode ?? string.Empty;
                txtCostCenter.Text = slaMaster.CostCenter ?? string.Empty;
                txtStartDate.Text = slaMaster.StartDate?.ToString("yyyy-MM-dd") ?? string.Empty;
                txtEndDate.Text = slaMaster.EndDate?.ToString("yyyy-MM-dd") ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddSlaMasterAsync()
    {
        try
        {
            var request = new SlaMasterCreateRequest
            {
                AutomationID = ParseNullableInt(txtAutomationID.Text),
                ComplexityID = ParseNullableInt(txtComplexityID.Text),
                MedalID = ParseNullableInt(txtMedalID.Text),
                Zcode = string.IsNullOrWhiteSpace(txtZcode.Text) ? null : txtZcode.Text.Trim(),
                CostCenter = string.IsNullOrWhiteSpace(txtCostCenter.Text) ? null : txtCostCenter.Text.Trim(),
                StartDate = ParseNullableDateTime(txtStartDate.Text),
                EndDate = ParseNullableDateTime(txtEndDate.Text)
            };

            await _slaMasterApiClient.CreateAsync(request);
            MessageBox.Show("SLA Master created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadSlaMastersAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating SLA master: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateSlaMasterAsync()
    {
        try
        {
            if (_selectedSlaMasterId == null)
            {
                MessageBox.Show("Please select a SLA master to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new SlaMasterUpdateRequest
            {
                AutomationID = ParseNullableInt(txtAutomationID.Text),
                ComplexityID = ParseNullableInt(txtComplexityID.Text),
                MedalID = ParseNullableInt(txtMedalID.Text),
                Zcode = string.IsNullOrWhiteSpace(txtZcode.Text) ? null : txtZcode.Text.Trim(),
                CostCenter = string.IsNullOrWhiteSpace(txtCostCenter.Text) ? null : txtCostCenter.Text.Trim(),
                StartDate = ParseNullableDateTime(txtStartDate.Text),
                EndDate = ParseNullableDateTime(txtEndDate.Text)
            };

            await _slaMasterApiClient.UpdateAsync(_selectedSlaMasterId.Value, request);
            MessageBox.Show("SLA Master updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadSlaMastersAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating SLA master: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteSlaMasterAsync()
    {
        try
        {
            if (_selectedSlaMasterId == null)
            {
                MessageBox.Show("Please select a SLA master to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this SLA master?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                await _slaMasterApiClient.DeleteAsync(_selectedSlaMasterId.Value);
                MessageBox.Show("SLA Master deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadSlaMastersAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting SLA master: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedSlaMasterId = null;
        txtAutomationID.Text = string.Empty;
        txtComplexityID.Text = string.Empty;
        txtMedalID.Text = string.Empty;
        txtZcode.Text = string.Empty;
        txtCostCenter.Text = string.Empty;
        txtStartDate.Text = string.Empty;
        txtEndDate.Text = string.Empty;
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

    private DateTime? ParseNullableDateTime(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (DateTime.TryParse(value.Trim(), out DateTime result))
            return result;

        return null;
    }
}
