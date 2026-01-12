using RpaAlmClient.Models;
using RpaAlmClient.Services;

namespace RpaAlmClient.Forms;

public partial class SlaLogEntryManagementForm : Form
{
    private readonly SlaLogEntryApiClient _slaLogEntryApiClient;
    private DataGridView dgvSlaLogEntries = null!;
    private TextBox txtSlaMasterID = null!;
    private TextBox txtCreatedWWID = null!;
    private TextBox txtCreatedDate = null!;
    private TextBox txtComment = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblSlaMasterID = null!;
    private Label lblCreatedWWID = null!;
    private Label lblCreatedDate = null!;
    private Label lblComment = null!;
    private int? _selectedSlaLogEntryId;

    public SlaLogEntryManagementForm()
    {
        _slaLogEntryApiClient = new SlaLogEntryApiClient();
        InitializeComponents();
        _ = LoadSlaLogEntriesAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - SLA Log Entry Management";
        this.Size = new Size(950, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvSlaLogEntries = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 500),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvSlaLogEntries.SelectionChanged += DgvSlaLogEntries_SelectionChanged;

        // Labels and TextBoxes
        int labelX = 580;
        int textBoxX = 710;
        int startY = 30;
        int yIncrement = 40;
        int currentY = startY;

        lblSlaMasterID = new Label { Text = "SlaMasterID:", Location = new Point(labelX, currentY), Size = new Size(120, 23) };
        txtSlaMasterID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(210, 23) };
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

        btnAdd.Click += async (s, e) => await AddSlaLogEntryAsync();
        btnUpdate.Click += async (s, e) => await UpdateSlaLogEntryAsync();
        btnDelete.Click += async (s, e) => await DeleteSlaLogEntryAsync();
        btnRefresh.Click += async (s, e) => await LoadSlaLogEntriesAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvSlaLogEntries,
            lblSlaMasterID, lblCreatedWWID, lblCreatedDate, lblComment,
            txtSlaMasterID, txtCreatedWWID, txtCreatedDate, txtComment,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadSlaLogEntriesAsync()
    {
        try
        {
            var slaLogEntries = await _slaLogEntryApiClient.GetAllAsync();
            dgvSlaLogEntries.DataSource = slaLogEntries;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading SLA log entries: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvSlaLogEntries_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvSlaLogEntries.SelectedRows.Count > 0)
        {
            var row = dgvSlaLogEntries.SelectedRows[0];
            var slaLogEntry = row.DataBoundItem as SlaLogEntryDto;

            if (slaLogEntry != null)
            {
                _selectedSlaLogEntryId = slaLogEntry.Id;
                txtSlaMasterID.Text = slaLogEntry.SlaMasterID?.ToString() ?? string.Empty;
                txtCreatedWWID.Text = slaLogEntry.CreatedWWID ?? string.Empty;
                txtCreatedDate.Text = slaLogEntry.CreatedDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty;
                txtComment.Text = slaLogEntry.Comment ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddSlaLogEntryAsync()
    {
        try
        {
            var request = new SlaLogEntryCreateRequest
            {
                SlaMasterID = ParseNullableInt(txtSlaMasterID.Text),
                CreatedWWID = string.IsNullOrWhiteSpace(txtCreatedWWID.Text) ? null : txtCreatedWWID.Text.Trim(),
                CreatedDate = ParseNullableDateTime(txtCreatedDate.Text),
                Comment = string.IsNullOrWhiteSpace(txtComment.Text) ? null : txtComment.Text.Trim()
            };

            await _slaLogEntryApiClient.CreateAsync(request);
            MessageBox.Show("SLA Log Entry created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadSlaLogEntriesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating SLA log entry: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateSlaLogEntryAsync()
    {
        try
        {
            if (_selectedSlaLogEntryId == null)
            {
                MessageBox.Show("Please select a SLA log entry to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new SlaLogEntryUpdateRequest
            {
                SlaMasterID = ParseNullableInt(txtSlaMasterID.Text),
                CreatedWWID = string.IsNullOrWhiteSpace(txtCreatedWWID.Text) ? null : txtCreatedWWID.Text.Trim(),
                CreatedDate = ParseNullableDateTime(txtCreatedDate.Text),
                Comment = string.IsNullOrWhiteSpace(txtComment.Text) ? null : txtComment.Text.Trim()
            };

            await _slaLogEntryApiClient.UpdateAsync(_selectedSlaLogEntryId.Value, request);
            MessageBox.Show("SLA Log Entry updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadSlaLogEntriesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating SLA log entry: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteSlaLogEntryAsync()
    {
        try
        {
            if (_selectedSlaLogEntryId == null)
            {
                MessageBox.Show("Please select a SLA log entry to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this SLA log entry?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                await _slaLogEntryApiClient.DeleteAsync(_selectedSlaLogEntryId.Value);
                MessageBox.Show("SLA Log Entry deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadSlaLogEntriesAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting SLA log entry: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedSlaLogEntryId = null;
        txtSlaMasterID.Text = string.Empty;
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
