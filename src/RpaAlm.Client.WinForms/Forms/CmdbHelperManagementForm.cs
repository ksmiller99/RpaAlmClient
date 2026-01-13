using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.ApiClient.Clients;

namespace RpaAlm.Client.WinForms.Forms;

public partial class CmdbHelperManagementForm : Form
{
    private readonly CmdbHelperApiClient _cmdbHelperApiClient;
    private DataGridView dgvCmdbHelpers = null!;
    private TextBox txtAppId = null!;
    private TextBox txtName = null!;
    private TextBox txtZcode = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblAppId = null!;
    private Label lblName = null!;
    private Label lblZcode = null!;
    private string? _selectedAppId;

    public CmdbHelperManagementForm()
    {
        _cmdbHelperApiClient = new CmdbHelperApiClient();
        InitializeComponents();
        _ = LoadCmdbHelpersAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - CMDB Helper Management";
        this.Size = new Size(950, 500);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvCmdbHelpers = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 400),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvCmdbHelpers.SelectionChanged += DgvCmdbHelpers_SelectionChanged;

        // Labels and TextBoxes
        int labelX = 580;
        int textBoxX = 700;
        int startY = 30;
        int yIncrement = 40;
        int currentY = startY;

        lblAppId = new Label { Text = "AppId (PK):", Location = new Point(labelX, currentY), Size = new Size(110, 23) };
        txtAppId = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23), MaxLength = 50 };
        currentY += yIncrement;

        lblName = new Label { Text = "Name:", Location = new Point(labelX, currentY), Size = new Size(110, 23) };
        txtName = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23), MaxLength = 255 };
        currentY += yIncrement;

        lblZcode = new Label { Text = "Zcode:", Location = new Point(labelX, currentY), Size = new Size(110, 23) };
        txtZcode = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23), MaxLength = 50 };
        currentY += yIncrement;

        // Buttons
        btnAdd = new Button { Text = "Add New", Location = new Point(580, currentY + 20), Size = new Size(100, 30) };
        btnUpdate = new Button { Text = "Update", Location = new Point(690, currentY + 20), Size = new Size(100, 30), Enabled = false };
        btnDelete = new Button { Text = "Delete", Location = new Point(800, currentY + 20), Size = new Size(70, 30), Enabled = false };
        btnRefresh = new Button { Text = "Refresh", Location = new Point(580, currentY + 60), Size = new Size(100, 30) };

        btnAdd.Click += async (s, e) => await AddCmdbHelperAsync();
        btnUpdate.Click += async (s, e) => await UpdateCmdbHelperAsync();
        btnDelete.Click += async (s, e) => await DeleteCmdbHelperAsync();
        btnRefresh.Click += async (s, e) => await LoadCmdbHelpersAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvCmdbHelpers,
            lblAppId, lblName, lblZcode,
            txtAppId, txtName, txtZcode,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadCmdbHelpersAsync()
    {
        try
        {
            var cmdbHelpers = await _cmdbHelperApiClient.GetAllAsync();
            dgvCmdbHelpers.DataSource = cmdbHelpers;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading CMDB helpers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvCmdbHelpers_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvCmdbHelpers.SelectedRows.Count > 0)
        {
            var row = dgvCmdbHelpers.SelectedRows[0];
            var cmdbHelper = row.DataBoundItem as CmdbHelperDto;

            if (cmdbHelper != null)
            {
                _selectedAppId = cmdbHelper.AppId;
                txtAppId.Text = cmdbHelper.AppId ?? string.Empty;
                txtAppId.Enabled = false; // Disable editing primary key
                txtName.Text = cmdbHelper.Name ?? string.Empty;
                txtZcode.Text = cmdbHelper.Zcode ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddCmdbHelperAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtAppId.Text))
            {
                MessageBox.Show("AppId is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new CmdbHelperCreateRequest
            {
                AppId = txtAppId.Text.Trim(),
                Name = string.IsNullOrWhiteSpace(txtName.Text) ? null : txtName.Text.Trim(),
                Zcode = string.IsNullOrWhiteSpace(txtZcode.Text) ? null : txtZcode.Text.Trim()
            };

            await _cmdbHelperApiClient.CreateAsync(request);
            MessageBox.Show("CMDB Helper created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadCmdbHelpersAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating CMDB helper: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateCmdbHelperAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_selectedAppId))
            {
                MessageBox.Show("Please select a CMDB helper to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new CmdbHelperUpdateRequest
            {
                Name = string.IsNullOrWhiteSpace(txtName.Text) ? null : txtName.Text.Trim(),
                Zcode = string.IsNullOrWhiteSpace(txtZcode.Text) ? null : txtZcode.Text.Trim()
            };

            // Note: API client uses int id parameter, but we need to pass the AppId
            // This may need adjustment based on actual API implementation
            // For now, attempting to parse AppId as int (may fail if API doesn't support)
            if (int.TryParse(_selectedAppId, out int appIdAsInt))
            {
                await _cmdbHelperApiClient.UpdateAsync(appIdAsInt, request);
                MessageBox.Show("CMDB Helper updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadCmdbHelpersAsync();
            }
            else
            {
                MessageBox.Show("Unable to update: AppId must be numeric for API call.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating CMDB helper: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteCmdbHelperAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_selectedAppId))
            {
                MessageBox.Show("Please select a CMDB helper to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this CMDB helper?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // Note: API client uses int id parameter, but we need to pass the AppId
                // This may need adjustment based on actual API implementation
                if (int.TryParse(_selectedAppId, out int appIdAsInt))
                {
                    await _cmdbHelperApiClient.DeleteAsync(appIdAsInt);
                    MessageBox.Show("CMDB Helper deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadCmdbHelpersAsync();
                }
                else
                {
                    MessageBox.Show("Unable to delete: AppId must be numeric for API call.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting CMDB helper: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedAppId = null;
        txtAppId.Text = string.Empty;
        txtAppId.Enabled = true; // Re-enable for new entries
        txtName.Text = string.Empty;
        txtZcode.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnDelete.Enabled = false;
    }
}
