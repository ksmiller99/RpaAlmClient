using RpaAlmClient.Models;
using RpaAlmClient.Services;

namespace RpaAlmClient.Forms;

public partial class RegionManagementForm : Form
{
    private readonly RegionApiClient _regionApiClient;
    private DataGridView dgvRegions = null!;
    private TextBox txtCode = null!;
    private TextBox txtDescription = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblCode = null!;
    private Label lblDescription = null!;
    private int? _selectedRegionId;

    public RegionManagementForm()
    {
        _regionApiClient = new RegionApiClient();
        InitializeComponents();
        _ = LoadRegionsAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - Region Management";
        this.Size = new Size(900, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvRegions = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 500),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvRegions.SelectionChanged += DgvRegions_SelectionChanged;

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

        btnAdd.Click += async (s, e) => await AddRegionAsync();
        btnUpdate.Click += async (s, e) => await UpdateRegionAsync();
        btnDelete.Click += async (s, e) => await DeleteRegionAsync();
        btnRefresh.Click += async (s, e) => await LoadRegionsAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvRegions, lblCode, lblDescription,
            txtCode, txtDescription,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadRegionsAsync()
    {
        try
        {
            var regions = await _regionApiClient.GetAllAsync();
            dgvRegions.DataSource = regions;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading regions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvRegions_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvRegions.SelectedRows.Count > 0)
        {
            var row = dgvRegions.SelectedRows[0];
            var region = row.DataBoundItem as RegionDto;

            if (region != null)
            {
                _selectedRegionId = region.Id;
                txtCode.Text = region.Code ?? string.Empty;
                txtDescription.Text = region.Description ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddRegionAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new RegionCreateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _regionApiClient.CreateAsync(request);
            MessageBox.Show("Region created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadRegionsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating region: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateRegionAsync()
    {
        try
        {
            if (_selectedRegionId == null)
            {
                MessageBox.Show("Please select a region to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new RegionUpdateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _regionApiClient.UpdateAsync(_selectedRegionId.Value, request);
            MessageBox.Show("Region updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadRegionsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating region: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteRegionAsync()
    {
        try
        {
            if (_selectedRegionId == null)
            {
                MessageBox.Show("Please select a region to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                await _regionApiClient.DeleteAsync(_selectedRegionId.Value);
                MessageBox.Show("Region deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadRegionsAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting region: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedRegionId = null;
        txtCode.Text = string.Empty;
        txtDescription.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnDelete.Enabled = false;
    }
}
