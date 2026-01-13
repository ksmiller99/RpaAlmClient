using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.ApiClient.Clients;

namespace RpaAlm.Client.WinForms.Forms;

public partial class SlaItemManagementForm : Form
{
    private readonly SlaItemApiClient _slaItemApiClient;
    private readonly SlaMasterApiClient _slaMasterApiClient;
    private readonly SlaItemTypeApiClient _slaItemTypeApiClient;
    private readonly EnhancementApiClient _enhancementApiClient;
    private DataGridView dgvSlaItems = null!;
    private ComboBox cmbSlaMaster = null!;
    private ComboBox cmbSlaItemType = null!;
    private ComboBox cmbEnhancement = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblSlaMasterID = null!;
    private Label lblSlaItemTypeID = null!;
    private Label lblEnhancementID = null!;
    private int? _selectedSlaItemId;

    public SlaItemManagementForm()
    {
        _slaItemApiClient = new SlaItemApiClient();
        _slaMasterApiClient = new SlaMasterApiClient();
        _slaItemTypeApiClient = new SlaItemTypeApiClient();
        _enhancementApiClient = new EnhancementApiClient();

        InitializeComponents();
        _ = LoadLookupDataAsync();
        _ = LoadSlaItemsAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - SLA Item Management";
        this.Size = new Size(900, 500);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvSlaItems = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 400),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvSlaItems.SelectionChanged += DgvSlaItems_SelectionChanged;

        // Labels and TextBoxes
        int labelX = 580;
        int textBoxX = 720;
        int startY = 30;
        int yIncrement = 40;
        int currentY = startY;

        lblSlaMasterID = new Label { Text = "SLA Master:", Location = new Point(labelX, currentY), Size = new Size(130, 23) };
        cmbSlaMaster = new ComboBox
        {
            Location = new Point(textBoxX, currentY),
            Size = new Size(150, 23),
            DropDownStyle = ComboBoxStyle.DropDownList,
            DisplayMember = "Id",
            ValueMember = "Id"
        };
        currentY += yIncrement;

        lblSlaItemTypeID = new Label { Text = "Item Type:", Location = new Point(labelX, currentY), Size = new Size(130, 23) };
        cmbSlaItemType = new ComboBox
        {
            Location = new Point(textBoxX, currentY),
            Size = new Size(150, 23),
            DropDownStyle = ComboBoxStyle.DropDownList,
            DisplayMember = "Code",
            ValueMember = "Id"
        };
        currentY += yIncrement;

        lblEnhancementID = new Label { Text = "Enhancement:", Location = new Point(labelX, currentY), Size = new Size(130, 23) };
        cmbEnhancement = new ComboBox
        {
            Location = new Point(textBoxX, currentY),
            Size = new Size(150, 23),
            DropDownStyle = ComboBoxStyle.DropDownList,
            DisplayMember = "Code",
            ValueMember = "Id"
        };
        currentY += yIncrement;

        // Buttons
        btnAdd = new Button { Text = "Add New", Location = new Point(580, currentY + 20), Size = new Size(100, 30) };
        btnUpdate = new Button { Text = "Update", Location = new Point(690, currentY + 20), Size = new Size(100, 30), Enabled = false };
        btnDelete = new Button { Text = "Delete", Location = new Point(800, currentY + 20), Size = new Size(70, 30), Enabled = false };
        btnRefresh = new Button { Text = "Refresh", Location = new Point(580, currentY + 60), Size = new Size(100, 30) };

        btnAdd.Click += async (s, e) => await AddSlaItemAsync();
        btnUpdate.Click += async (s, e) => await UpdateSlaItemAsync();
        btnDelete.Click += async (s, e) => await DeleteSlaItemAsync();
        btnRefresh.Click += async (s, e) => await LoadSlaItemsAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvSlaItems,
            lblSlaMasterID, lblSlaItemTypeID, lblEnhancementID,
            cmbSlaMaster, cmbSlaItemType, cmbEnhancement,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadSlaItemsAsync()
    {
        try
        {
            var slaItems = await _slaItemApiClient.GetAllAsync();
            dgvSlaItems.DataSource = slaItems;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading SLA items: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task LoadLookupDataAsync()
    {
        try
        {
            var slaMasters = await _slaMasterApiClient.GetAllAsync();
            var slaItemTypes = await _slaItemTypeApiClient.GetAllAsync();
            var enhancements = await _enhancementApiClient.GetAllAsync();

            // Add empty option at the beginning
            slaMasters.Insert(0, new SlaMasterDto { Id = 0 });
            slaItemTypes.Insert(0, new SlaItemTypeDto { Id = 0, Code = "(None)" });
            enhancements.Insert(0, new EnhancementDto { Id = 0, Code = "(None)" });

            cmbSlaMaster.DataSource = slaMasters;
            cmbSlaItemType.DataSource = slaItemTypes;
            cmbEnhancement.DataSource = enhancements;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading lookup data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvSlaItems_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvSlaItems.SelectedRows.Count > 0)
        {
            var row = dgvSlaItems.SelectedRows[0];
            var slaItem = row.DataBoundItem as SlaItemDto;

            if (slaItem != null)
            {
                _selectedSlaItemId = slaItem.Id;
                cmbSlaMaster.SelectedValue = slaItem.SlaMasterID ?? 0;
                cmbSlaItemType.SelectedValue = slaItem.SlaItemTypeID ?? 0;
                cmbEnhancement.SelectedValue = slaItem.EnhancementID ?? 0;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddSlaItemAsync()
    {
        try
        {
            var request = new SlaItemCreateRequest
            {
                SlaMasterID = GetComboBoxValue(cmbSlaMaster),
                SlaItemTypeID = GetComboBoxValue(cmbSlaItemType),
                EnhancementID = GetComboBoxValue(cmbEnhancement)
            };

            await _slaItemApiClient.CreateAsync(request);
            MessageBox.Show("SLA Item created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadSlaItemsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating SLA item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateSlaItemAsync()
    {
        try
        {
            if (_selectedSlaItemId == null)
            {
                MessageBox.Show("Please select a SLA item to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new SlaItemUpdateRequest
            {
                SlaMasterID = GetComboBoxValue(cmbSlaMaster),
                SlaItemTypeID = GetComboBoxValue(cmbSlaItemType),
                EnhancementID = GetComboBoxValue(cmbEnhancement)
            };

            await _slaItemApiClient.UpdateAsync(_selectedSlaItemId.Value, request);
            MessageBox.Show("SLA Item updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadSlaItemsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating SLA item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteSlaItemAsync()
    {
        try
        {
            if (_selectedSlaItemId == null)
            {
                MessageBox.Show("Please select a SLA item to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this SLA item?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                await _slaItemApiClient.DeleteAsync(_selectedSlaItemId.Value);
                MessageBox.Show("SLA Item deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadSlaItemsAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting SLA item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedSlaItemId = null;
        cmbSlaMaster.SelectedIndex = 0;
        cmbSlaItemType.SelectedIndex = 0;
        cmbEnhancement.SelectedIndex = 0;
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

    private int? GetComboBoxValue(ComboBox combo)
    {
        if (combo.SelectedValue == null || (int)combo.SelectedValue == 0)
            return null;
        return (int)combo.SelectedValue;
    }
}
