using RpaAlmClient.Models;
using RpaAlmClient.Services;

namespace RpaAlmClient.Forms;

public partial class VirtualIdentityManagementForm : Form
{
    private readonly VirtualIdentityApiClient _virtualIdentityApiClient;
    private DataGridView dgvVirtualIdentities = null!;
    private TextBox txtAccountName = null!;
    private TextBox txtHostName = null!;
    private TextBox txtWWID = null!;
    private TextBox txtIPv4 = null!;
    private TextBox txtADDomainID = null!;
    private TextBox txtEmail = null!;
    private TextBox txtCreated = null!;
    private TextBox txtRetired = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblAccountName = null!;
    private Label lblHostName = null!;
    private Label lblWWID = null!;
    private Label lblIPv4 = null!;
    private Label lblADDomainID = null!;
    private Label lblEmail = null!;
    private Label lblCreated = null!;
    private Label lblRetired = null!;
    private int? _selectedVirtualIdentityId;

    public VirtualIdentityManagementForm()
    {
        _virtualIdentityApiClient = new VirtualIdentityApiClient();
        InitializeComponents();
        _ = LoadVirtualIdentitiesAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - Virtual Identity Management";
        this.Size = new Size(1050, 650);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvVirtualIdentities = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(600, 550),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvVirtualIdentities.SelectionChanged += DgvVirtualIdentities_SelectionChanged;

        // Labels and TextBoxes
        int labelX = 640;
        int textBoxX = 780;
        int startY = 30;
        int yIncrement = 40;
        int currentY = startY;

        lblAccountName = new Label { Text = "AccountName:", Location = new Point(labelX, currentY), Size = new Size(130, 23) };
        txtAccountName = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(240, 23), MaxLength = 255 };
        currentY += yIncrement;

        lblHostName = new Label { Text = "HostName:", Location = new Point(labelX, currentY), Size = new Size(130, 23) };
        txtHostName = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(240, 23), MaxLength = 255 };
        currentY += yIncrement;

        lblWWID = new Label { Text = "WWID:", Location = new Point(labelX, currentY), Size = new Size(130, 23) };
        txtWWID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(240, 23), MaxLength = 9 };
        currentY += yIncrement;

        lblIPv4 = new Label { Text = "IPv4:", Location = new Point(labelX, currentY), Size = new Size(130, 23) };
        txtIPv4 = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(240, 23), MaxLength = 15 };
        currentY += yIncrement;

        lblADDomainID = new Label { Text = "ADDomainID:", Location = new Point(labelX, currentY), Size = new Size(130, 23) };
        txtADDomainID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(240, 23) };
        currentY += yIncrement;

        lblEmail = new Label { Text = "Email:", Location = new Point(labelX, currentY), Size = new Size(130, 23) };
        txtEmail = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(240, 23), MaxLength = 255 };
        currentY += yIncrement;

        lblCreated = new Label { Text = "Created:", Location = new Point(labelX, currentY), Size = new Size(130, 23) };
        txtCreated = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(240, 23) };
        currentY += yIncrement;

        lblRetired = new Label { Text = "Retired:", Location = new Point(labelX, currentY), Size = new Size(130, 23) };
        txtRetired = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(240, 23) };
        currentY += yIncrement;

        // Buttons
        btnAdd = new Button { Text = "Add New", Location = new Point(640, currentY + 20), Size = new Size(100, 30) };
        btnUpdate = new Button { Text = "Update", Location = new Point(750, currentY + 20), Size = new Size(100, 30), Enabled = false };
        btnDelete = new Button { Text = "Delete", Location = new Point(860, currentY + 20), Size = new Size(80, 30), Enabled = false };
        btnRefresh = new Button { Text = "Refresh", Location = new Point(950, currentY + 20), Size = new Size(80, 30) };

        btnAdd.Click += async (s, e) => await AddVirtualIdentityAsync();
        btnUpdate.Click += async (s, e) => await UpdateVirtualIdentityAsync();
        btnDelete.Click += async (s, e) => await DeleteVirtualIdentityAsync();
        btnRefresh.Click += async (s, e) => await LoadVirtualIdentitiesAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvVirtualIdentities,
            lblAccountName, lblHostName, lblWWID, lblIPv4, lblADDomainID, lblEmail, lblCreated, lblRetired,
            txtAccountName, txtHostName, txtWWID, txtIPv4, txtADDomainID, txtEmail, txtCreated, txtRetired,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadVirtualIdentitiesAsync()
    {
        try
        {
            var virtualIdentities = await _virtualIdentityApiClient.GetAllAsync();
            dgvVirtualIdentities.DataSource = virtualIdentities;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading virtual identities: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvVirtualIdentities_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvVirtualIdentities.SelectedRows.Count > 0)
        {
            var row = dgvVirtualIdentities.SelectedRows[0];
            var virtualIdentity = row.DataBoundItem as VirtualIdentityDto;

            if (virtualIdentity != null)
            {
                _selectedVirtualIdentityId = virtualIdentity.Id;
                txtAccountName.Text = virtualIdentity.AccountName ?? string.Empty;
                txtHostName.Text = virtualIdentity.HostName ?? string.Empty;
                txtWWID.Text = virtualIdentity.WWID ?? string.Empty;
                txtIPv4.Text = virtualIdentity.IPv4 ?? string.Empty;
                txtADDomainID.Text = virtualIdentity.ADDomainID?.ToString() ?? string.Empty;
                txtEmail.Text = virtualIdentity.Email ?? string.Empty;
                txtCreated.Text = virtualIdentity.Created?.ToString("yyyy-MM-dd") ?? string.Empty;
                txtRetired.Text = virtualIdentity.Retired?.ToString("yyyy-MM-dd") ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddVirtualIdentityAsync()
    {
        try
        {
            var request = new VirtualIdentityCreateRequest
            {
                AccountName = string.IsNullOrWhiteSpace(txtAccountName.Text) ? null : txtAccountName.Text.Trim(),
                HostName = string.IsNullOrWhiteSpace(txtHostName.Text) ? null : txtHostName.Text.Trim(),
                WWID = string.IsNullOrWhiteSpace(txtWWID.Text) ? null : txtWWID.Text.Trim(),
                IPv4 = string.IsNullOrWhiteSpace(txtIPv4.Text) ? null : txtIPv4.Text.Trim(),
                ADDomainID = ParseNullableInt(txtADDomainID.Text),
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                Created = ParseNullableDateTime(txtCreated.Text),
                Retired = ParseNullableDateTime(txtRetired.Text)
            };

            await _virtualIdentityApiClient.CreateAsync(request);
            MessageBox.Show("Virtual Identity created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadVirtualIdentitiesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating virtual identity: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateVirtualIdentityAsync()
    {
        try
        {
            if (_selectedVirtualIdentityId == null)
            {
                MessageBox.Show("Please select a virtual identity to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new VirtualIdentityUpdateRequest
            {
                AccountName = string.IsNullOrWhiteSpace(txtAccountName.Text) ? null : txtAccountName.Text.Trim(),
                HostName = string.IsNullOrWhiteSpace(txtHostName.Text) ? null : txtHostName.Text.Trim(),
                WWID = string.IsNullOrWhiteSpace(txtWWID.Text) ? null : txtWWID.Text.Trim(),
                IPv4 = string.IsNullOrWhiteSpace(txtIPv4.Text) ? null : txtIPv4.Text.Trim(),
                ADDomainID = ParseNullableInt(txtADDomainID.Text),
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                Created = ParseNullableDateTime(txtCreated.Text),
                Retired = ParseNullableDateTime(txtRetired.Text)
            };

            await _virtualIdentityApiClient.UpdateAsync(_selectedVirtualIdentityId.Value, request);
            MessageBox.Show("Virtual Identity updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadVirtualIdentitiesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating virtual identity: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteVirtualIdentityAsync()
    {
        try
        {
            if (_selectedVirtualIdentityId == null)
            {
                MessageBox.Show("Please select a virtual identity to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this virtual identity?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                await _virtualIdentityApiClient.DeleteAsync(_selectedVirtualIdentityId.Value);
                MessageBox.Show("Virtual Identity deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadVirtualIdentitiesAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting virtual identity: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedVirtualIdentityId = null;
        txtAccountName.Text = string.Empty;
        txtHostName.Text = string.Empty;
        txtWWID.Text = string.Empty;
        txtIPv4.Text = string.Empty;
        txtADDomainID.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtCreated.Text = string.Empty;
        txtRetired.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnDelete.Enabled = false;
    }

    private int? ParseNullableInt(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (int.TryParse(value.Trim(), out int result))
            return result;

        return null;
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
