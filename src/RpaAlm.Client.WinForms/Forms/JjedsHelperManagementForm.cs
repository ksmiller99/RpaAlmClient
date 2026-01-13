using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.ApiClient.Clients;

namespace RpaAlm.Client.WinForms.Forms;

public partial class JjedsHelperManagementForm : Form
{
    private readonly JjedsHelperApiClient _jjedsHelperApiClient;
    private DataGridView dgvJjedsHelpers = null!;
    private TextBox txtWwid = null!;
    private TextBox txtCommonName = null!;
    private TextBox txtEmail = null!;
    private TextBox txtJjedsCreated = null!;
    private TextBox txtJjedsLastChanged = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblWwid = null!;
    private Label lblCommonName = null!;
    private Label lblEmail = null!;
    private Label lblJjedsCreated = null!;
    private Label lblJjedsLastChanged = null!;
    private string? _selectedWwid;

    public JjedsHelperManagementForm()
    {
        _jjedsHelperApiClient = new JjedsHelperApiClient();
        InitializeComponents();
        _ = LoadJjedsHelpersAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - JJEDS Helper Management";
        this.Size = new Size(1000, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvJjedsHelpers = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 500),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvJjedsHelpers.SelectionChanged += DgvJjedsHelpers_SelectionChanged;

        // Labels and TextBoxes
        int labelX = 580;
        int textBoxX = 730;
        int startY = 30;
        int yIncrement = 40;
        int currentY = startY;

        lblWwid = new Label { Text = "Wwid (PK):", Location = new Point(labelX, currentY), Size = new Size(140, 23) };
        txtWwid = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(230, 23), MaxLength = 9 };
        currentY += yIncrement;

        lblCommonName = new Label { Text = "CommonName:", Location = new Point(labelX, currentY), Size = new Size(140, 23) };
        txtCommonName = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(230, 23), MaxLength = 255 };
        currentY += yIncrement;

        lblEmail = new Label { Text = "Email:", Location = new Point(labelX, currentY), Size = new Size(140, 23) };
        txtEmail = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(230, 23), MaxLength = 255 };
        currentY += yIncrement;

        lblJjedsCreated = new Label { Text = "JjedsCreated:", Location = new Point(labelX, currentY), Size = new Size(140, 23) };
        txtJjedsCreated = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(230, 23) };
        currentY += yIncrement;

        lblJjedsLastChanged = new Label { Text = "JjedsLastChanged:", Location = new Point(labelX, currentY), Size = new Size(140, 23) };
        txtJjedsLastChanged = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(230, 23) };
        currentY += yIncrement;

        // Buttons
        btnAdd = new Button { Text = "Add New", Location = new Point(580, currentY + 20), Size = new Size(100, 30) };
        btnUpdate = new Button { Text = "Update", Location = new Point(690, currentY + 20), Size = new Size(100, 30), Enabled = false };
        btnDelete = new Button { Text = "Delete", Location = new Point(800, currentY + 20), Size = new Size(80, 30), Enabled = false };
        btnRefresh = new Button { Text = "Refresh", Location = new Point(890, currentY + 20), Size = new Size(80, 30) };

        btnAdd.Click += async (s, e) => await AddJjedsHelperAsync();
        btnUpdate.Click += async (s, e) => await UpdateJjedsHelperAsync();
        btnDelete.Click += async (s, e) => await DeleteJjedsHelperAsync();
        btnRefresh.Click += async (s, e) => await LoadJjedsHelpersAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvJjedsHelpers,
            lblWwid, lblCommonName, lblEmail, lblJjedsCreated, lblJjedsLastChanged,
            txtWwid, txtCommonName, txtEmail, txtJjedsCreated, txtJjedsLastChanged,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadJjedsHelpersAsync()
    {
        try
        {
            var jjedsHelpers = await _jjedsHelperApiClient.GetAllAsync();
            dgvJjedsHelpers.DataSource = jjedsHelpers;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading JJEDS helpers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvJjedsHelpers_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvJjedsHelpers.SelectedRows.Count > 0)
        {
            var row = dgvJjedsHelpers.SelectedRows[0];
            var jjedsHelper = row.DataBoundItem as JjedsHelperDto;

            if (jjedsHelper != null)
            {
                _selectedWwid = jjedsHelper.Wwid;
                txtWwid.Text = jjedsHelper.Wwid ?? string.Empty;
                txtWwid.Enabled = false; // Disable editing primary key
                txtCommonName.Text = jjedsHelper.CommonName ?? string.Empty;
                txtEmail.Text = jjedsHelper.Email ?? string.Empty;
                txtJjedsCreated.Text = jjedsHelper.JjedsCreated?.ToString("yyyy-MM-dd") ?? string.Empty;
                txtJjedsLastChanged.Text = jjedsHelper.JjedsLastChanged?.ToString("yyyy-MM-dd") ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddJjedsHelperAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtWwid.Text))
            {
                MessageBox.Show("Wwid is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new JjedsHelperCreateRequest
            {
                Wwid = txtWwid.Text.Trim(),
                CommonName = string.IsNullOrWhiteSpace(txtCommonName.Text) ? null : txtCommonName.Text.Trim(),
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                JjedsCreated = ParseNullableDateTime(txtJjedsCreated.Text),
                JjedsLastChanged = ParseNullableDateTime(txtJjedsLastChanged.Text)
            };

            await _jjedsHelperApiClient.CreateAsync(request);
            MessageBox.Show("JJEDS Helper created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadJjedsHelpersAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating JJEDS helper: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateJjedsHelperAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_selectedWwid))
            {
                MessageBox.Show("Please select a JJEDS helper to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new JjedsHelperUpdateRequest
            {
                CommonName = string.IsNullOrWhiteSpace(txtCommonName.Text) ? null : txtCommonName.Text.Trim(),
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                JjedsCreated = ParseNullableDateTime(txtJjedsCreated.Text),
                JjedsLastChanged = ParseNullableDateTime(txtJjedsLastChanged.Text)
            };

            // Note: API client uses int id parameter, but we need to pass the Wwid
            // This may need adjustment based on actual API implementation
            // For now, attempting to parse Wwid as int (may fail if API doesn't support)
            if (int.TryParse(_selectedWwid, out int wwidAsInt))
            {
                await _jjedsHelperApiClient.UpdateAsync(wwidAsInt, request);
                MessageBox.Show("JJEDS Helper updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadJjedsHelpersAsync();
            }
            else
            {
                MessageBox.Show("Unable to update: Wwid must be numeric for API call.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating JJEDS helper: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteJjedsHelperAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_selectedWwid))
            {
                MessageBox.Show("Please select a JJEDS helper to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this JJEDS helper?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // Note: API client uses int id parameter, but we need to pass the Wwid
                // This may need adjustment based on actual API implementation
                if (int.TryParse(_selectedWwid, out int wwidAsInt))
                {
                    await _jjedsHelperApiClient.DeleteAsync(wwidAsInt);
                    MessageBox.Show("JJEDS Helper deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadJjedsHelpersAsync();
                }
                else
                {
                    MessageBox.Show("Unable to delete: Wwid must be numeric for API call.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting JJEDS helper: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedWwid = null;
        txtWwid.Text = string.Empty;
        txtWwid.Enabled = true; // Re-enable for new entries
        txtCommonName.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtJjedsCreated.Text = string.Empty;
        txtJjedsLastChanged.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnDelete.Enabled = false;
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
