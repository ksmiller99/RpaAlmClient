using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.ApiClient.Clients;

namespace RpaAlm.Client.WinForms.Forms;

public partial class RpaStatusManagementForm : Form
{
    private readonly RpaStatusApiClient _rpaStatusApiClient;
    private DataGridView dgvRpaStatuses = null!;
    private TextBox txtCode = null!;
    private TextBox txtDescription = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblCode = null!;
    private Label lblDescription = null!;
    private int? _selectedRpaStatusId;

    public RpaStatusManagementForm()
    {
        _rpaStatusApiClient = new RpaStatusApiClient();
        InitializeComponents();
        _ = LoadRpaStatusesAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - RPA Status Management";
        this.Size = new Size(900, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvRpaStatuses = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 500),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvRpaStatuses.SelectionChanged += DgvRpaStatusesSelectionChanged;

        // Labels
        lblCode = new Label { Text = "Code:", Location = new Point(580, 30), Size = new Size(100, 23) };
        lblDescription = new Label { Text = "Description:", Location = new Point(580, 70), Size = new Size(100, 23) };

        // TextBoxes
        txtCode = new TextBox { Location = new Point(690, 30), Size = new Size(180, 23), MaxLength = 50 };
        txtDescription = new TextBox { Location = new Point(690, 70), Size = new Size(180, 100), Multiline = true, MaxLength = 255 };

        // Buttons
        btnAdd = new Button { Text = "Add New", Location = new Point(580, 190), Size = new Size(100, 30) };
        btnUpdate = new Button { Text = "Update", Location = new Point(690, 190), Size = new Size(100, 30), Enabled = false };
        btnDelete = new Button { Text = "Delete", Location = new Point(800, 190), Size = new Size(70, 30), Enabled = false };
        btnRefresh = new Button { Text = "Refresh", Location = new Point(580, 230), Size = new Size(100, 30) };

        btnAdd.Click += async (s, e) => await AddRpaStatusAsync();
        btnUpdate.Click += async (s, e) => await UpdateRpaStatusAsync();
        btnDelete.Click += async (s, e) => await DeleteRpaStatusAsync();
        btnRefresh.Click += async (s, e) => await LoadRpaStatusesAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvRpaStatuses, lblCode, lblDescription,
            txtCode, txtDescription,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadRpaStatusesAsync()
    {
        try
        {
            var rpaStatuses = await _rpaStatusApiClient.GetAllAsync();
            dgvRpaStatuses.DataSource = rpaStatuses;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading RpaStatuses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvRpaStatusesSelectionChanged(object? sender, EventArgs e)
    {
        if (dgvRpaStatuses.SelectedRows.Count > 0)
        {
            var row = dgvRpaStatuses.SelectedRows[0];
            var status = row.DataBoundItem as RpaStatusDto;

            if (status != null)
            {
                _selectedRpaStatusId = status.Id;
                txtCode.Text = status.Code ?? string.Empty;
                txtDescription.Text = status.Description ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddRpaStatusAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new RpaStatusCreateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _rpaStatusApiClient.CreateAsync(request);
            MessageBox.Show("Status created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadRpaStatusesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateRpaStatusAsync()
    {
        try
        {
            if (_selectedRpaStatusId == null)
            {
                MessageBox.Show("Please select a status to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new RpaStatusUpdateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _rpaStatusApiClient.UpdateAsync(_selectedRpaStatusId.Value, request);
            MessageBox.Show("Status updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadRpaStatusesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteRpaStatusAsync()
    {
        try
        {
            if (_selectedRpaStatusId == null)
            {
                MessageBox.Show("Please select a status to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this status?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                await _rpaStatusApiClient.DeleteAsync(_selectedRpaStatusId.Value);
                MessageBox.Show("Status deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadRpaStatusesAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedRpaStatusId = null;
        txtCode.Text = string.Empty;
        txtDescription.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnDelete.Enabled = false;
    }
}
