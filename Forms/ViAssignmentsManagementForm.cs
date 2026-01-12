using RpaAlmClient.Models;
using RpaAlmClient.Services;

namespace RpaAlmClient.Forms;

public partial class ViAssignmentsManagementForm : Form
{
    private readonly ViAssignmentsApiClient _viAssignmentsApiClient;
    private DataGridView dgvViAssignments = null!;
    private TextBox txtVirtualIdentityID = null!;
    private TextBox txtAutomationEnvironmentID = null!;
    private TextBox txtPercentage = null!;
    private TextBox txtStartDate = null!;
    private TextBox txtEndDate = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblVirtualIdentityID = null!;
    private Label lblAutomationEnvironmentID = null!;
    private Label lblPercentage = null!;
    private Label lblStartDate = null!;
    private Label lblEndDate = null!;
    private int? _selectedViAssignmentId;

    public ViAssignmentsManagementForm()
    {
        _viAssignmentsApiClient = new ViAssignmentsApiClient();
        InitializeComponents();
        _ = LoadViAssignmentsAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - VI Assignments Management";
        this.Size = new Size(1000, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvViAssignments = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 500),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvViAssignments.SelectionChanged += DgvViAssignments_SelectionChanged;

        // Labels and TextBoxes
        int labelX = 580;
        int textBoxX = 790;
        int startY = 30;
        int yIncrement = 40;
        int currentY = startY;

        lblVirtualIdentityID = new Label { Text = "VirtualIdentityID:", Location = new Point(labelX, currentY), Size = new Size(200, 23) };
        txtVirtualIdentityID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(180, 23) };
        currentY += yIncrement;

        lblAutomationEnvironmentID = new Label { Text = "AutomationEnvironmentID:", Location = new Point(labelX, currentY), Size = new Size(200, 23) };
        txtAutomationEnvironmentID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(180, 23) };
        currentY += yIncrement;

        lblPercentage = new Label { Text = "Percentage:", Location = new Point(labelX, currentY), Size = new Size(200, 23) };
        txtPercentage = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(180, 23) };
        currentY += yIncrement;

        lblStartDate = new Label { Text = "StartDate:", Location = new Point(labelX, currentY), Size = new Size(200, 23) };
        txtStartDate = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(180, 23) };
        currentY += yIncrement;

        lblEndDate = new Label { Text = "EndDate:", Location = new Point(labelX, currentY), Size = new Size(200, 23) };
        txtEndDate = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(180, 23) };
        currentY += yIncrement;

        // Buttons
        btnAdd = new Button { Text = "Add New", Location = new Point(580, currentY + 20), Size = new Size(100, 30) };
        btnUpdate = new Button { Text = "Update", Location = new Point(690, currentY + 20), Size = new Size(100, 30), Enabled = false };
        btnDelete = new Button { Text = "Delete", Location = new Point(800, currentY + 20), Size = new Size(80, 30), Enabled = false };
        btnRefresh = new Button { Text = "Refresh", Location = new Point(890, currentY + 20), Size = new Size(80, 30) };

        btnAdd.Click += async (s, e) => await AddViAssignmentAsync();
        btnUpdate.Click += async (s, e) => await UpdateViAssignmentAsync();
        btnDelete.Click += async (s, e) => await DeleteViAssignmentAsync();
        btnRefresh.Click += async (s, e) => await LoadViAssignmentsAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvViAssignments,
            lblVirtualIdentityID, lblAutomationEnvironmentID, lblPercentage, lblStartDate, lblEndDate,
            txtVirtualIdentityID, txtAutomationEnvironmentID, txtPercentage, txtStartDate, txtEndDate,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadViAssignmentsAsync()
    {
        try
        {
            var viAssignments = await _viAssignmentsApiClient.GetAllAsync();
            dgvViAssignments.DataSource = viAssignments;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading VI assignments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvViAssignments_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvViAssignments.SelectedRows.Count > 0)
        {
            var row = dgvViAssignments.SelectedRows[0];
            var viAssignment = row.DataBoundItem as ViAssignmentsDto;

            if (viAssignment != null)
            {
                _selectedViAssignmentId = viAssignment.Id;
                txtVirtualIdentityID.Text = viAssignment.VirtualIdentityID?.ToString() ?? string.Empty;
                txtAutomationEnvironmentID.Text = viAssignment.AutomationEnvironmentID?.ToString() ?? string.Empty;
                txtPercentage.Text = viAssignment.Percentage?.ToString() ?? string.Empty;
                txtStartDate.Text = viAssignment.StartDate?.ToString("yyyy-MM-dd") ?? string.Empty;
                txtEndDate.Text = viAssignment.EndDate?.ToString("yyyy-MM-dd") ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddViAssignmentAsync()
    {
        try
        {
            var request = new ViAssignmentsCreateRequest
            {
                VirtualIdentityID = ParseNullableInt(txtVirtualIdentityID.Text),
                AutomationEnvironmentID = ParseNullableInt(txtAutomationEnvironmentID.Text),
                Percentage = ParseNullableInt(txtPercentage.Text),
                StartDate = ParseNullableDateTime(txtStartDate.Text),
                EndDate = ParseNullableDateTime(txtEndDate.Text)
            };

            await _viAssignmentsApiClient.CreateAsync(request);
            MessageBox.Show("VI Assignment created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadViAssignmentsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating VI assignment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateViAssignmentAsync()
    {
        try
        {
            if (_selectedViAssignmentId == null)
            {
                MessageBox.Show("Please select a VI assignment to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new ViAssignmentsUpdateRequest
            {
                VirtualIdentityID = ParseNullableInt(txtVirtualIdentityID.Text),
                AutomationEnvironmentID = ParseNullableInt(txtAutomationEnvironmentID.Text),
                Percentage = ParseNullableInt(txtPercentage.Text),
                StartDate = ParseNullableDateTime(txtStartDate.Text),
                EndDate = ParseNullableDateTime(txtEndDate.Text)
            };

            await _viAssignmentsApiClient.UpdateAsync(_selectedViAssignmentId.Value, request);
            MessageBox.Show("VI Assignment updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadViAssignmentsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating VI assignment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteViAssignmentAsync()
    {
        try
        {
            if (_selectedViAssignmentId == null)
            {
                MessageBox.Show("Please select a VI assignment to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this VI assignment?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                await _viAssignmentsApiClient.DeleteAsync(_selectedViAssignmentId.Value);
                MessageBox.Show("VI Assignment deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadViAssignmentsAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting VI assignment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedViAssignmentId = null;
        txtVirtualIdentityID.Text = string.Empty;
        txtAutomationEnvironmentID.Text = string.Empty;
        txtPercentage.Text = string.Empty;
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
