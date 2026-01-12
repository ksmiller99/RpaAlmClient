using RpaAlmClient.Models;
using RpaAlmClient.Services;

namespace RpaAlmClient.Forms;

public partial class FunctionManagementForm : Form
{
    private readonly FunctionApiClient _functionApiClient;
    private DataGridView dgvFunctions = null!;
    private TextBox txtCode = null!;
    private TextBox txtDescription = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblCode = null!;
    private Label lblDescription = null!;
    private int? _selectedFunctionId;

    public FunctionManagementForm()
    {
        _functionApiClient = new FunctionApiClient();
        InitializeComponents();
        _ = LoadFunctionsAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - Function Management";
        this.Size = new Size(900, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvFunctions = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 500),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvFunctions.SelectionChanged += DgvFunctions_SelectionChanged;

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

        btnAdd.Click += async (s, e) => await AddFunctionAsync();
        btnUpdate.Click += async (s, e) => await UpdateFunctionAsync();
        btnDelete.Click += async (s, e) => await DeleteFunctionAsync();
        btnRefresh.Click += async (s, e) => await LoadFunctionsAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvFunctions, lblCode, lblDescription,
            txtCode, txtDescription,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadFunctionsAsync()
    {
        try
        {
            var functions = await _functionApiClient.GetAllAsync();
            dgvFunctions.DataSource = functions;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading functions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvFunctions_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvFunctions.SelectedRows.Count > 0)
        {
            var row = dgvFunctions.SelectedRows[0];
            var function = row.DataBoundItem as FunctionDto;

            if (function != null)
            {
                _selectedFunctionId = function.Id;
                txtCode.Text = function.Code ?? string.Empty;
                txtDescription.Text = function.Description ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddFunctionAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new FunctionCreateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _functionApiClient.CreateAsync(request);
            MessageBox.Show("Function created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadFunctionsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating function: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateFunctionAsync()
    {
        try
        {
            if (_selectedFunctionId == null)
            {
                MessageBox.Show("Please select a function to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new FunctionUpdateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _functionApiClient.UpdateAsync(_selectedFunctionId.Value, request);
            MessageBox.Show("Function updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadFunctionsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating function: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteFunctionAsync()
    {
        try
        {
            if (_selectedFunctionId == null)
            {
                MessageBox.Show("Please select a function to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                await _functionApiClient.DeleteAsync(_selectedFunctionId.Value);
                MessageBox.Show("Function deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadFunctionsAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting function: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedFunctionId = null;
        txtCode.Text = string.Empty;
        txtDescription.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnDelete.Enabled = false;
    }
}
