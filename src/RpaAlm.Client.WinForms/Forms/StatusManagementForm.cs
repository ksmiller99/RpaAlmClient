using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.ApiClient.Clients;

namespace RpaAlm.Client.WinForms.Forms;

public partial class StatusManagementForm : Form
{
    private readonly StatusApiClient _statusApiClient;
    private DataGridView dgvStatuses = null!;
    private TextBox txtCode = null!;
    private TextBox txtDescription = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblCode = null!;
    private Label lblDescription = null!;
    private int? _selectedStatusId;

    public StatusManagementForm()
    {
        _statusApiClient = new StatusApiClient();
        InitializeComponents();
        _ = LoadStatusesAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - Status Management";
        this.Size = new Size(900, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvStatuses = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 500),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvStatuses.SelectionChanged += DgvStatuses_SelectionChanged;

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

        btnAdd.Click += async (s, e) => await AddStatusAsync();
        btnUpdate.Click += async (s, e) => await UpdateStatusAsync();
        btnDelete.Click += async (s, e) => await DeleteStatusAsync();
        btnRefresh.Click += async (s, e) => await LoadStatusesAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvStatuses, lblCode, lblDescription,
            txtCode, txtDescription,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadStatusesAsync()
    {
        try
        {
            var statuses = await _statusApiClient.GetAllAsync();
            dgvStatuses.DataSource = statuses;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading statuses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvStatuses_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvStatuses.SelectedRows.Count > 0)
        {
            var row = dgvStatuses.SelectedRows[0];
            var status = row.DataBoundItem as StatusDto;

            if (status != null)
            {
                _selectedStatusId = status.Id;
                txtCode.Text = status.Code ?? string.Empty;
                txtDescription.Text = status.Description ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddStatusAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new StatusCreateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _statusApiClient.CreateAsync(request);
            MessageBox.Show("Status created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadStatusesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateStatusAsync()
    {
        try
        {
            if (_selectedStatusId == null)
            {
                MessageBox.Show("Please select a status to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new StatusUpdateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _statusApiClient.UpdateAsync(_selectedStatusId.Value, request);
            MessageBox.Show("Status updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadStatusesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteStatusAsync()
    {
        try
        {
            if (_selectedStatusId == null)
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
                await _statusApiClient.DeleteAsync(_selectedStatusId.Value);
                MessageBox.Show("Status deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadStatusesAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedStatusId = null;
        txtCode.Text = string.Empty;
        txtDescription.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnDelete.Enabled = false;
    }
}
