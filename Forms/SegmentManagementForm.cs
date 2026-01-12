using RpaAlmClient.Models;
using RpaAlmClient.Services;

namespace RpaAlmClient.Forms;

public partial class SegmentManagementForm : Form
{
    private readonly SegmentApiClient _segmentApiClient;
    private DataGridView dgvSegments = null!;
    private TextBox txtCode = null!;
    private TextBox txtDescription = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblCode = null!;
    private Label lblDescription = null!;
    private int? _selectedSegmentId;

    public SegmentManagementForm()
    {
        _segmentApiClient = new SegmentApiClient();
        InitializeComponents();
        _ = LoadSegmentsAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - Segment Management";
        this.Size = new Size(900, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvSegments = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 500),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvSegments.SelectionChanged += DgvSegments_SelectionChanged;

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

        btnAdd.Click += async (s, e) => await AddSegmentAsync();
        btnUpdate.Click += async (s, e) => await UpdateSegmentAsync();
        btnDelete.Click += async (s, e) => await DeleteSegmentAsync();
        btnRefresh.Click += async (s, e) => await LoadSegmentsAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvSegments, lblCode, lblDescription,
            txtCode, txtDescription,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadSegmentsAsync()
    {
        try
        {
            var segments = await _segmentApiClient.GetAllAsync();
            dgvSegments.DataSource = segments;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading segments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvSegments_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvSegments.SelectedRows.Count > 0)
        {
            var row = dgvSegments.SelectedRows[0];
            var segment = row.DataBoundItem as SegmentDto;

            if (segment != null)
            {
                _selectedSegmentId = segment.Id;
                txtCode.Text = segment.Code ?? string.Empty;
                txtDescription.Text = segment.Description ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddSegmentAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new SegmentCreateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _segmentApiClient.CreateAsync(request);
            MessageBox.Show("Segment created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadSegmentsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating segment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateSegmentAsync()
    {
        try
        {
            if (_selectedSegmentId == null)
            {
                MessageBox.Show("Please select a segment to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new SegmentUpdateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _segmentApiClient.UpdateAsync(_selectedSegmentId.Value, request);
            MessageBox.Show("Segment updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadSegmentsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating segment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteSegmentAsync()
    {
        try
        {
            if (_selectedSegmentId == null)
            {
                MessageBox.Show("Please select a segment to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                await _segmentApiClient.DeleteAsync(_selectedSegmentId.Value);
                MessageBox.Show("Segment deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadSegmentsAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting segment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedSegmentId = null;
        txtCode.Text = string.Empty;
        txtDescription.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnDelete.Enabled = false;
    }
}
