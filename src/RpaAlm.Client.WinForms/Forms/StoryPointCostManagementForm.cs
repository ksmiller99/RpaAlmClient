using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.ApiClient.Clients;

namespace RpaAlm.Client.WinForms.Forms;

public partial class StoryPointCostManagementForm : Form
{
    private readonly StoryPointCostApiClient _storyPointCostApiClient;
    private DataGridView dgvStoryPointCosts = null!;
    private TextBox txtCode = null!;
    private TextBox txtDescription = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblCode = null!;
    private Label lblDescription = null!;
    private int? _selectedStoryPointCostId;

    public StoryPointCostManagementForm()
    {
        _storyPointCostApiClient = new StoryPointCostApiClient();
        InitializeComponents();
        _ = LoadStoryPointCostsAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - StoryPointCost Management";
        this.Size = new Size(900, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvStoryPointCosts = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 500),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvStoryPointCosts.SelectionChanged += DgvStoryPointCosts_SelectionChanged;

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

        btnAdd.Click += async (s, e) => await AddStoryPointCostAsync();
        btnUpdate.Click += async (s, e) => await UpdateStoryPointCostAsync();
        btnDelete.Click += async (s, e) => await DeleteStoryPointCostAsync();
        btnRefresh.Click += async (s, e) => await LoadStoryPointCostsAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvStoryPointCosts, lblCode, lblDescription,
            txtCode, txtDescription,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadStoryPointCostsAsync()
    {
        try
        {
            var storypointcosts = await _storyPointCostApiClient.GetAllAsync();
            dgvStoryPointCosts.DataSource = storypointcosts;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading storypointcosts: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvStoryPointCosts_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvStoryPointCosts.SelectedRows.Count > 0)
        {
            var row = dgvStoryPointCosts.SelectedRows[0];
            var storyPointCost = row.DataBoundItem as StoryPointCostDto;

            if (storyPointCost != null)
            {
                _selectedStoryPointCostId = storyPointCost.Id;
                txtCode.Text = storyPointCost.Code ?? string.Empty;
                txtDescription.Text = storyPointCost.Description ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddStoryPointCostAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new StoryPointCostCreateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _storyPointCostApiClient.CreateAsync(request);
            MessageBox.Show("StoryPointCost created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadStoryPointCostsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating storyPointCost: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateStoryPointCostAsync()
    {
        try
        {
            if (_selectedStoryPointCostId == null)
            {
                MessageBox.Show("Please select a storyPointCost to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new StoryPointCostUpdateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _storyPointCostApiClient.UpdateAsync(_selectedStoryPointCostId.Value, request);
            MessageBox.Show("StoryPointCost updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadStoryPointCostsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating storyPointCost: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteStoryPointCostAsync()
    {
        try
        {
            if (_selectedStoryPointCostId == null)
            {
                MessageBox.Show("Please select a storyPointCost to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                await _storyPointCostApiClient.DeleteAsync(_selectedStoryPointCostId.Value);
                MessageBox.Show("StoryPointCost deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadStoryPointCostsAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting storyPointCost: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedStoryPointCostId = null;
        txtCode.Text = string.Empty;
        txtDescription.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnDelete.Enabled = false;
    }
}
