using RpaAlmClient.Models;
using RpaAlmClient.Services;

namespace RpaAlmClient.Forms;

public partial class ADDomainManagementForm : Form
{
    private readonly ADDomainApiClient _aDDomainApiClient;
    private DataGridView dgvADDomains = null!;
    private TextBox txtCode = null!;
    private TextBox txtDescription = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblCode = null!;
    private Label lblDescription = null!;
    private int? _selectedADDomainId;

    public ADDomainManagementForm()
    {
        _aDDomainApiClient = new ADDomainApiClient();
        InitializeComponents();
        _ = LoadADDomainsAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - ADDomain Management";
        this.Size = new Size(900, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvADDomains = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 500),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvADDomains.SelectionChanged += DgvADDomains_SelectionChanged;

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

        btnAdd.Click += async (s, e) => await AddADDomainAsync();
        btnUpdate.Click += async (s, e) => await UpdateADDomainAsync();
        btnDelete.Click += async (s, e) => await DeleteADDomainAsync();
        btnRefresh.Click += async (s, e) => await LoadADDomainsAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvADDomains, lblCode, lblDescription,
            txtCode, txtDescription,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadADDomainsAsync()
    {
        try
        {
            var addomains = await _aDDomainApiClient.GetAllAsync();
            dgvADDomains.DataSource = addomains;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading addomains: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvADDomains_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvADDomains.SelectedRows.Count > 0)
        {
            var row = dgvADDomains.SelectedRows[0];
            var aDDomain = row.DataBoundItem as ADDomainDto;

            if (aDDomain != null)
            {
                _selectedADDomainId = aDDomain.Id;
                txtCode.Text = aDDomain.Code ?? string.Empty;
                txtDescription.Text = aDDomain.Description ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddADDomainAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new ADDomainCreateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _aDDomainApiClient.CreateAsync(request);
            MessageBox.Show("ADDomain created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadADDomainsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating aDDomain: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateADDomainAsync()
    {
        try
        {
            if (_selectedADDomainId == null)
            {
                MessageBox.Show("Please select a aDDomain to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new ADDomainUpdateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _aDDomainApiClient.UpdateAsync(_selectedADDomainId.Value, request);
            MessageBox.Show("ADDomain updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadADDomainsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating aDDomain: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteADDomainAsync()
    {
        try
        {
            if (_selectedADDomainId == null)
            {
                MessageBox.Show("Please select a aDDomain to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                await _aDDomainApiClient.DeleteAsync(_selectedADDomainId.Value);
                MessageBox.Show("ADDomain deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadADDomainsAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting aDDomain: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedADDomainId = null;
        txtCode.Text = string.Empty;
        txtDescription.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnDelete.Enabled = false;
    }
}
