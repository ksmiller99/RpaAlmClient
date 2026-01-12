using RpaAlmClient.Models;
using RpaAlmClient.Services;

namespace RpaAlmClient.Forms;

public partial class AutomationEnvironmentTypeManagementForm : Form
{
    private readonly AutomationEnvironmentTypeApiClient _automationEnvironmentTypeApiClient;
    private DataGridView dgvAutomationEnvironmentTypes = null!;
    private TextBox txtCode = null!;
    private TextBox txtDescription = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblCode = null!;
    private Label lblDescription = null!;
    private int? _selectedAutomationEnvironmentTypeId;

    public AutomationEnvironmentTypeManagementForm()
    {
        _automationEnvironmentTypeApiClient = new AutomationEnvironmentTypeApiClient();
        InitializeComponents();
        _ = LoadAutomationEnvironmentTypesAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - AutomationEnvironmentType Management";
        this.Size = new Size(900, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvAutomationEnvironmentTypes = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 500),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvAutomationEnvironmentTypes.SelectionChanged += DgvAutomationEnvironmentTypes_SelectionChanged;

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

        btnAdd.Click += async (s, e) => await AddAutomationEnvironmentTypeAsync();
        btnUpdate.Click += async (s, e) => await UpdateAutomationEnvironmentTypeAsync();
        btnDelete.Click += async (s, e) => await DeleteAutomationEnvironmentTypeAsync();
        btnRefresh.Click += async (s, e) => await LoadAutomationEnvironmentTypesAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvAutomationEnvironmentTypes, lblCode, lblDescription,
            txtCode, txtDescription,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadAutomationEnvironmentTypesAsync()
    {
        try
        {
            var automationenvironmenttypes = await _automationEnvironmentTypeApiClient.GetAllAsync();
            dgvAutomationEnvironmentTypes.DataSource = automationenvironmenttypes;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading automationenvironmenttypes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvAutomationEnvironmentTypes_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvAutomationEnvironmentTypes.SelectedRows.Count > 0)
        {
            var row = dgvAutomationEnvironmentTypes.SelectedRows[0];
            var automationEnvironmentType = row.DataBoundItem as AutomationEnvironmentTypeDto;

            if (automationEnvironmentType != null)
            {
                _selectedAutomationEnvironmentTypeId = automationEnvironmentType.Id;
                txtCode.Text = automationEnvironmentType.Code ?? string.Empty;
                txtDescription.Text = automationEnvironmentType.Description ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddAutomationEnvironmentTypeAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new AutomationEnvironmentTypeCreateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _automationEnvironmentTypeApiClient.CreateAsync(request);
            MessageBox.Show("AutomationEnvironmentType created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadAutomationEnvironmentTypesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating automationEnvironmentType: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateAutomationEnvironmentTypeAsync()
    {
        try
        {
            if (_selectedAutomationEnvironmentTypeId == null)
            {
                MessageBox.Show("Please select a automationEnvironmentType to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new AutomationEnvironmentTypeUpdateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _automationEnvironmentTypeApiClient.UpdateAsync(_selectedAutomationEnvironmentTypeId.Value, request);
            MessageBox.Show("AutomationEnvironmentType updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadAutomationEnvironmentTypesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating automationEnvironmentType: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteAutomationEnvironmentTypeAsync()
    {
        try
        {
            if (_selectedAutomationEnvironmentTypeId == null)
            {
                MessageBox.Show("Please select a automationEnvironmentType to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                await _automationEnvironmentTypeApiClient.DeleteAsync(_selectedAutomationEnvironmentTypeId.Value);
                MessageBox.Show("AutomationEnvironmentType deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadAutomationEnvironmentTypesAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting automationEnvironmentType: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedAutomationEnvironmentTypeId = null;
        txtCode.Text = string.Empty;
        txtDescription.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnDelete.Enabled = false;
    }
}
