using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.ApiClient.Clients;

namespace RpaAlm.Client.WinForms.Forms;

public partial class SlaItemTypeManagementForm : Form
{
    private readonly SlaItemTypeApiClient _slaItemTypeApiClient;
    private DataGridView dgvSlaItemTypes = null!;
    private TextBox txtCode = null!;
    private TextBox txtDescription = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblCode = null!;
    private Label lblDescription = null!;
    private int? _selectedSlaItemTypeId;

    public SlaItemTypeManagementForm()
    {
        _slaItemTypeApiClient = new SlaItemTypeApiClient();
        InitializeComponents();
        _ = LoadSlaItemTypesAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - SlaItemType Management";
        this.Size = new Size(900, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvSlaItemTypes = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 500),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvSlaItemTypes.SelectionChanged += DgvSlaItemTypes_SelectionChanged;

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

        btnAdd.Click += async (s, e) => await AddSlaItemTypeAsync();
        btnUpdate.Click += async (s, e) => await UpdateSlaItemTypeAsync();
        btnDelete.Click += async (s, e) => await DeleteSlaItemTypeAsync();
        btnRefresh.Click += async (s, e) => await LoadSlaItemTypesAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvSlaItemTypes, lblCode, lblDescription,
            txtCode, txtDescription,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadSlaItemTypesAsync()
    {
        try
        {
            var slaitemtypes = await _slaItemTypeApiClient.GetAllAsync();
            dgvSlaItemTypes.DataSource = slaitemtypes;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading slaitemtypes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvSlaItemTypes_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvSlaItemTypes.SelectedRows.Count > 0)
        {
            var row = dgvSlaItemTypes.SelectedRows[0];
            var slaItemType = row.DataBoundItem as SlaItemTypeDto;

            if (slaItemType != null)
            {
                _selectedSlaItemTypeId = slaItemType.Id;
                txtCode.Text = slaItemType.Code ?? string.Empty;
                txtDescription.Text = slaItemType.Description ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddSlaItemTypeAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new SlaItemTypeCreateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _slaItemTypeApiClient.CreateAsync(request);
            MessageBox.Show("SlaItemType created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadSlaItemTypesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating slaItemType: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateSlaItemTypeAsync()
    {
        try
        {
            if (_selectedSlaItemTypeId == null)
            {
                MessageBox.Show("Please select a slaItemType to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new SlaItemTypeUpdateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _slaItemTypeApiClient.UpdateAsync(_selectedSlaItemTypeId.Value, request);
            MessageBox.Show("SlaItemType updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadSlaItemTypesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating slaItemType: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteSlaItemTypeAsync()
    {
        try
        {
            if (_selectedSlaItemTypeId == null)
            {
                MessageBox.Show("Please select a slaItemType to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this ",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                await _slaItemTypeApiClient.DeleteAsync(_selectedSlaItemTypeId.Value);
                MessageBox.Show("SlaItemType deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadSlaItemTypesAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting slaItemType: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedSlaItemTypeId = null;
        txtCode.Text = string.Empty;
        txtDescription.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnDelete.Enabled = false;
    }
}
