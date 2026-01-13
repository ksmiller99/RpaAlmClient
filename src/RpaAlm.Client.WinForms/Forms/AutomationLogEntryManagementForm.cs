using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.ApiClient.Clients;

namespace RpaAlm.Client.WinForms.Forms;

public partial class AutomationLogEntryManagementForm : Form
{
    private readonly AutomationLogEntryApiClient _automationLogEntryApiClient;
    private DataGridView dgvAutomationLogEntries = null!;
    private TextBox txtAutomationID = null!;
    private TextBox txtCreatedWWID = null!;
    private TextBox txtCreatedDate = null!;
    private TextBox txtComment = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblAutomationID = null!;
    private Label lblCreatedWWID = null!;
    private Label lblCreatedDate = null!;
    private Label lblComment = null!;
    private int? _selectedAutomationLogEntryId;

    public AutomationLogEntryManagementForm()
    {
        _automationLogEntryApiClient = new AutomationLogEntryApiClient();
        InitializeComponents();
        _ = LoadAutomationLogEntriesAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - Automation Log Entry Management";
        this.Size = new Size(950, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvAutomationLogEntries = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 500),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvAutomationLogEntries.SelectionChanged += DgvAutomationLogEntries_SelectionChanged;

        // Labels and TextBoxes
        int labelX = 580;
        int textBoxX = 710;
        int startY = 30;
        int yIncrement = 40;
        int currentY = startY;

        lblAutomationID = new Label { Text = "AutomationID:", Location = new Point(labelX, currentY), Size = new Size(120, 23) };
        txtAutomationID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(210, 23) };
        currentY += yIncrement;

        lblCreatedWWID = new Label { Text = "CreatedWWID:", Location = new Point(labelX, currentY), Size = new Size(120, 23) };
        txtCreatedWWID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(210, 23), MaxLength = 9 };
        currentY += yIncrement;

        lblCreatedDate = new Label { Text = "CreatedDate:", Location = new Point(labelX, currentY), Size = new Size(120, 23) };
        txtCreatedDate = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(210, 23) };
        currentY += yIncrement;

        lblComment = new Label { Text = "Comment:", Location = new Point(labelX, currentY), Size = new Size(120, 23) };
        txtComment = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(210, 100), Multiline = true };
        currentY += 110;

        // Buttons
        btnAdd = new Button { Text = "Add New", Location = new Point(580, currentY + 20), Size = new Size(100, 30) };
        btnUpdate = new Button { Text = "Update", Location = new Point(690, currentY + 20), Size = new Size(100, 30), Enabled = false };
        btnDelete = new Button { Text = "Delete", Location = new Point(800, currentY + 20), Size = new Size(70, 30), Enabled = false };
        btnRefresh = new Button { Text = "Refresh", Location = new Point(580, currentY + 60), Size = new Size(100, 30) };

        btnAdd.Click += async (s, e) => await AddAutomationLogEntryAsync();
        btnUpdate.Click += async (s, e) => await UpdateAutomationLogEntryAsync();
        btnDelete.Click += async (s, e) => await DeleteAutomationLogEntryAsync();
        btnRefresh.Click += async (s, e) => await LoadAutomationLogEntriesAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvAutomationLogEntries,
            lblAutomationID, lblCreatedWWID, lblCreatedDate, lblComment,
            txtAutomationID, txtCreatedWWID, txtCreatedDate, txtComment,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadAutomationLogEntriesAsync()
    {
        try
        {
            var automationLogEntries = await _automationLogEntryApiClient.GetAllAsync();
            dgvAutomationLogEntries.DataSource = automationLogEntries;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading automation log entries: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvAutomationLogEntries_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvAutomationLogEntries.SelectedRows.Count > 0)
        {
            var row = dgvAutomationLogEntries.SelectedRows[0];
            var automationLogEntry = row.DataBoundItem as AutomationLogEntryDto;

            if (automationLogEntry != null)
            {
                _selectedAutomationLogEntryId = automationLogEntry.Id;
                txtAutomationID.Text = automationLogEntry.AutomationID?.ToString() ?? string.Empty;
                txtCreatedWWID.Text = automationLogEntry.CreatedWWID ?? string.Empty;
                txtCreatedDate.Text = automationLogEntry.CreatedDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty;
                txtComment.Text = automationLogEntry.Comment ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddAutomationLogEntryAsync()
    {
        try
        {
            var request = new AutomationLogEntryCreateRequest
            {
                AutomationID = ParseNullableInt(txtAutomationID.Text),
                CreatedWWID = string.IsNullOrWhiteSpace(txtCreatedWWID.Text) ? null : txtCreatedWWID.Text.Trim(),
                CreatedDate = ParseNullableDateTime(txtCreatedDate.Text),
                Comment = string.IsNullOrWhiteSpace(txtComment.Text) ? null : txtComment.Text.Trim()
            };

            await _automationLogEntryApiClient.CreateAsync(request);
            MessageBox.Show("Automation Log Entry created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadAutomationLogEntriesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating automation log entry: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateAutomationLogEntryAsync()
    {
        try
        {
            if (_selectedAutomationLogEntryId == null)
            {
                MessageBox.Show("Please select an automation log entry to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new AutomationLogEntryUpdateRequest
            {
                AutomationID = ParseNullableInt(txtAutomationID.Text),
                CreatedWWID = string.IsNullOrWhiteSpace(txtCreatedWWID.Text) ? null : txtCreatedWWID.Text.Trim(),
                CreatedDate = ParseNullableDateTime(txtCreatedDate.Text),
                Comment = string.IsNullOrWhiteSpace(txtComment.Text) ? null : txtComment.Text.Trim()
            };

            await _automationLogEntryApiClient.UpdateAsync(_selectedAutomationLogEntryId.Value, request);
            MessageBox.Show("Automation Log Entry updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadAutomationLogEntriesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating automation log entry: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteAutomationLogEntryAsync()
    {
        try
        {
            if (_selectedAutomationLogEntryId == null)
            {
                MessageBox.Show("Please select an automation log entry to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this automation log entry?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                await _automationLogEntryApiClient.DeleteAsync(_selectedAutomationLogEntryId.Value);
                MessageBox.Show("Automation Log Entry deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadAutomationLogEntriesAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting automation log entry: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedAutomationLogEntryId = null;
        txtAutomationID.Text = string.Empty;
        txtCreatedWWID.Text = string.Empty;
        txtCreatedDate.Text = string.Empty;
        txtComment.Text = string.Empty;
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
