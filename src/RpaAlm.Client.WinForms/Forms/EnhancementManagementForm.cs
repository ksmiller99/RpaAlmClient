using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.ApiClient.Clients;

namespace RpaAlm.Client.WinForms.Forms;

public partial class EnhancementManagementForm : Form
{
    private readonly EnhancementApiClient _enhancementApiClient;
    private DataGridView dgvEnhancements = null!;
    private TextBox txtCode = null!;
    private TextBox txtDescription = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblCode = null!;
    private Label lblDescription = null!;
    private int? _selectedEnhancementId;

    public EnhancementManagementForm()
    {
        _enhancementApiClient = new EnhancementApiClient();
        InitializeComponents();
        _ = LoadEnhancementsAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - Enhancement Management";
        this.Size = new Size(900, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvEnhancements = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 500),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvEnhancements.SelectionChanged += DgvEnhancements_SelectionChanged;

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

        btnAdd.Click += async (s, e) => await AddEnhancementAsync();
        btnUpdate.Click += async (s, e) => await UpdateEnhancementAsync();
        btnDelete.Click += async (s, e) => await DeleteEnhancementAsync();
        btnRefresh.Click += async (s, e) => await LoadEnhancementsAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvEnhancements, lblCode, lblDescription,
            txtCode, txtDescription,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadEnhancementsAsync()
    {
        try
        {
            var enhancements = await _enhancementApiClient.GetAllAsync();
            dgvEnhancements.DataSource = enhancements;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading enhancements: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvEnhancements_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvEnhancements.SelectedRows.Count > 0)
        {
            var row = dgvEnhancements.SelectedRows[0];
            var enhancement = row.DataBoundItem as EnhancementDto;

            if (enhancement != null)
            {
                _selectedEnhancementId = enhancement.Id;
                txtCode.Text = enhancement.Code ?? string.Empty;
                txtDescription.Text = enhancement.Description ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddEnhancementAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new EnhancementCreateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _enhancementApiClient.CreateAsync(request);
            MessageBox.Show("Enhancement created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadEnhancementsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating enhancement: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateEnhancementAsync()
    {
        try
        {
            if (_selectedEnhancementId == null)
            {
                MessageBox.Show("Please select a enhancement to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new EnhancementUpdateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _enhancementApiClient.UpdateAsync(_selectedEnhancementId.Value, request);
            MessageBox.Show("Enhancement updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadEnhancementsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating enhancement: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteEnhancementAsync()
    {
        try
        {
            if (_selectedEnhancementId == null)
            {
                MessageBox.Show("Please select a enhancement to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                await _enhancementApiClient.DeleteAsync(_selectedEnhancementId.Value);
                MessageBox.Show("Enhancement deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadEnhancementsAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting enhancement: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedEnhancementId = null;
        txtCode.Text = string.Empty;
        txtDescription.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnDelete.Enabled = false;
    }
}
