using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.ApiClient.Clients;

namespace RpaAlm.Client.WinForms.Forms;

public partial class ComplexityManagementForm : Form
{
    private readonly ComplexityApiClient _complexityApiClient;
    private DataGridView dgvComplexitys = null!;
    private TextBox txtCode = null!;
    private TextBox txtDescription = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblCode = null!;
    private Label lblDescription = null!;
    private int? _selectedComplexityId;

    public ComplexityManagementForm()
    {
        _complexityApiClient = new ComplexityApiClient();
        InitializeComponents();
        _ = LoadComplexitysAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - Complexity Management";
        this.Size = new Size(900, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvComplexitys = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 500),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvComplexitys.SelectionChanged += DgvComplexitys_SelectionChanged;

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

        btnAdd.Click += async (s, e) => await AddComplexityAsync();
        btnUpdate.Click += async (s, e) => await UpdateComplexityAsync();
        btnDelete.Click += async (s, e) => await DeleteComplexityAsync();
        btnRefresh.Click += async (s, e) => await LoadComplexitysAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvComplexitys, lblCode, lblDescription,
            txtCode, txtDescription,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadComplexitysAsync()
    {
        try
        {
            var complexitys = await _complexityApiClient.GetAllAsync();
            dgvComplexitys.DataSource = complexitys;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading complexitys: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvComplexitys_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvComplexitys.SelectedRows.Count > 0)
        {
            var row = dgvComplexitys.SelectedRows[0];
            var complexity = row.DataBoundItem as ComplexityDto;

            if (complexity != null)
            {
                _selectedComplexityId = complexity.Id;
                txtCode.Text = complexity.Code ?? string.Empty;
                txtDescription.Text = complexity.Description ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddComplexityAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new ComplexityCreateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _complexityApiClient.CreateAsync(request);
            MessageBox.Show("Complexity created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadComplexitysAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating complexity: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateComplexityAsync()
    {
        try
        {
            if (_selectedComplexityId == null)
            {
                MessageBox.Show("Please select a complexity to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new ComplexityUpdateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _complexityApiClient.UpdateAsync(_selectedComplexityId.Value, request);
            MessageBox.Show("Complexity updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadComplexitysAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating complexity: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteComplexityAsync()
    {
        try
        {
            if (_selectedComplexityId == null)
            {
                MessageBox.Show("Please select a complexity to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                await _complexityApiClient.DeleteAsync(_selectedComplexityId.Value);
                MessageBox.Show("Complexity deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadComplexitysAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting complexity: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedComplexityId = null;
        txtCode.Text = string.Empty;
        txtDescription.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnDelete.Enabled = false;
    }
}
