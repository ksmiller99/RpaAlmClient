using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.ApiClient.Clients;

namespace RpaAlm.Client.WinForms.Forms;

public partial class MedalManagementForm : Form
{
    private readonly MedalApiClient _medalApiClient;
    private DataGridView dgvMedals = null!;
    private TextBox txtCode = null!;
    private TextBox txtDescription = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblCode = null!;
    private Label lblDescription = null!;
    private int? _selectedMedalId;

    public MedalManagementForm()
    {
        _medalApiClient = new MedalApiClient();
        InitializeComponents();
        _ = LoadMedalsAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - Medal Management";
        this.Size = new Size(900, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvMedals = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 500),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvMedals.SelectionChanged += DgvMedals_SelectionChanged;

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

        btnAdd.Click += async (s, e) => await AddMedalAsync();
        btnUpdate.Click += async (s, e) => await UpdateMedalAsync();
        btnDelete.Click += async (s, e) => await DeleteMedalAsync();
        btnRefresh.Click += async (s, e) => await LoadMedalsAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvMedals, lblCode, lblDescription,
            txtCode, txtDescription,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadMedalsAsync()
    {
        try
        {
            var medals = await _medalApiClient.GetAllAsync();
            dgvMedals.DataSource = medals;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading medals: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvMedals_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvMedals.SelectedRows.Count > 0)
        {
            var row = dgvMedals.SelectedRows[0];
            var medal = row.DataBoundItem as MedalDto;

            if (medal != null)
            {
                _selectedMedalId = medal.Id;
                txtCode.Text = medal.Code ?? string.Empty;
                txtDescription.Text = medal.Description ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddMedalAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new MedalCreateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _medalApiClient.CreateAsync(request);
            MessageBox.Show("Medal created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadMedalsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating medal: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateMedalAsync()
    {
        try
        {
            if (_selectedMedalId == null)
            {
                MessageBox.Show("Please select a medal to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new MedalUpdateRequest
            {
                Code = txtCode.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
            };

            await _medalApiClient.UpdateAsync(_selectedMedalId.Value, request);
            MessageBox.Show("Medal updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadMedalsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating medal: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteMedalAsync()
    {
        try
        {
            if (_selectedMedalId == null)
            {
                MessageBox.Show("Please select a medal to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                await _medalApiClient.DeleteAsync(_selectedMedalId.Value);
                MessageBox.Show("Medal deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadMedalsAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting medal: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedMedalId = null;
        txtCode.Text = string.Empty;
        txtDescription.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnDelete.Enabled = false;
    }
}
