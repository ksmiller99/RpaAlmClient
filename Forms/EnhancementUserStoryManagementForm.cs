using RpaAlmClient.Models;
using RpaAlmClient.Services;

namespace RpaAlmClient.Forms;

public partial class EnhancementUserStoryManagementForm : Form
{
    private readonly EnhancementUserStoryApiClient _enhancementUserStoryApiClient;
    private DataGridView dgvEnhancementUserStories = null!;
    private TextBox txtEnhancementID = null!;
    private TextBox txtJiraIssue = null!;
    private TextBox txtStoryPoints = null!;
    private TextBox txtJiraIssueLink = null!;
    private TextBox txtJiraIssueSummary = null!;
    private TextBox txtStoryPointCostID = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Label lblEnhancementID = null!;
    private Label lblJiraIssue = null!;
    private Label lblStoryPoints = null!;
    private Label lblJiraIssueLink = null!;
    private Label lblJiraIssueSummary = null!;
    private Label lblStoryPointCostID = null!;
    private int? _selectedEnhancementUserStoryId;

    public EnhancementUserStoryManagementForm()
    {
        _enhancementUserStoryApiClient = new EnhancementUserStoryApiClient();
        InitializeComponents();
        _ = LoadEnhancementUserStoriesAsync();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM - Enhancement User Story Management";
        this.Size = new Size(1000, 650);
        this.StartPosition = FormStartPosition.CenterScreen;

        // DataGridView
        dgvEnhancementUserStories = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(540, 550),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        dgvEnhancementUserStories.SelectionChanged += DgvEnhancementUserStories_SelectionChanged;

        // Labels and TextBoxes
        int labelX = 580;
        int textBoxX = 750;
        int startY = 30;
        int yIncrement = 40;
        int currentY = startY;

        lblEnhancementID = new Label { Text = "EnhancementID:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtEnhancementID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23) };
        currentY += yIncrement;

        lblJiraIssue = new Label { Text = "JiraIssue:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtJiraIssue = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23), MaxLength = 50 };
        currentY += yIncrement;

        lblStoryPoints = new Label { Text = "StoryPoints:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtStoryPoints = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23) };
        currentY += yIncrement;

        lblJiraIssueLink = new Label { Text = "JiraIssueLink:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtJiraIssueLink = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23), MaxLength = 255 };
        currentY += yIncrement;

        lblJiraIssueSummary = new Label { Text = "JiraIssueSummary:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtJiraIssueSummary = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23), MaxLength = 255 };
        currentY += yIncrement;

        lblStoryPointCostID = new Label { Text = "StoryPointCostID:", Location = new Point(labelX, currentY), Size = new Size(160, 23) };
        txtStoryPointCostID = new TextBox { Location = new Point(textBoxX, currentY), Size = new Size(220, 23) };
        currentY += yIncrement;

        // Buttons
        btnAdd = new Button { Text = "Add New", Location = new Point(580, currentY + 20), Size = new Size(100, 30) };
        btnUpdate = new Button { Text = "Update", Location = new Point(690, currentY + 20), Size = new Size(100, 30), Enabled = false };
        btnDelete = new Button { Text = "Delete", Location = new Point(800, currentY + 20), Size = new Size(80, 30), Enabled = false };
        btnRefresh = new Button { Text = "Refresh", Location = new Point(890, currentY + 20), Size = new Size(80, 30) };

        btnAdd.Click += async (s, e) => await AddEnhancementUserStoryAsync();
        btnUpdate.Click += async (s, e) => await UpdateEnhancementUserStoryAsync();
        btnDelete.Click += async (s, e) => await DeleteEnhancementUserStoryAsync();
        btnRefresh.Click += async (s, e) => await LoadEnhancementUserStoriesAsync();

        // Add controls to form
        this.Controls.AddRange(new Control[]
        {
            dgvEnhancementUserStories,
            lblEnhancementID, lblJiraIssue, lblStoryPoints, lblJiraIssueLink, lblJiraIssueSummary, lblStoryPointCostID,
            txtEnhancementID, txtJiraIssue, txtStoryPoints, txtJiraIssueLink, txtJiraIssueSummary, txtStoryPointCostID,
            btnAdd, btnUpdate, btnDelete, btnRefresh
        });
    }

    private async Task LoadEnhancementUserStoriesAsync()
    {
        try
        {
            var enhancementUserStories = await _enhancementUserStoryApiClient.GetAllAsync();
            dgvEnhancementUserStories.DataSource = enhancementUserStories;
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading enhancement user stories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvEnhancementUserStories_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvEnhancementUserStories.SelectedRows.Count > 0)
        {
            var row = dgvEnhancementUserStories.SelectedRows[0];
            var enhancementUserStory = row.DataBoundItem as EnhancementUserStoryDto;

            if (enhancementUserStory != null)
            {
                _selectedEnhancementUserStoryId = enhancementUserStory.Id;
                txtEnhancementID.Text = enhancementUserStory.EnhancementID?.ToString() ?? string.Empty;
                txtJiraIssue.Text = enhancementUserStory.JiraIssue ?? string.Empty;
                txtStoryPoints.Text = enhancementUserStory.StoryPoints?.ToString() ?? string.Empty;
                txtJiraIssueLink.Text = enhancementUserStory.JiraIssueLink ?? string.Empty;
                txtJiraIssueSummary.Text = enhancementUserStory.JiraIssueSummary ?? string.Empty;
                txtStoryPointCostID.Text = enhancementUserStory.StoryPointCostID?.ToString() ?? string.Empty;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }

    private async Task AddEnhancementUserStoryAsync()
    {
        try
        {
            var request = new EnhancementUserStoryCreateRequest
            {
                EnhancementID = ParseNullableInt(txtEnhancementID.Text),
                JiraIssue = string.IsNullOrWhiteSpace(txtJiraIssue.Text) ? null : txtJiraIssue.Text.Trim(),
                StoryPoints = ParseNullableInt(txtStoryPoints.Text),
                JiraIssueLink = string.IsNullOrWhiteSpace(txtJiraIssueLink.Text) ? null : txtJiraIssueLink.Text.Trim(),
                JiraIssueSummary = string.IsNullOrWhiteSpace(txtJiraIssueSummary.Text) ? null : txtJiraIssueSummary.Text.Trim(),
                StoryPointCostID = ParseNullableInt(txtStoryPointCostID.Text)
            };

            await _enhancementUserStoryApiClient.CreateAsync(request);
            MessageBox.Show("Enhancement User Story created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadEnhancementUserStoriesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating enhancement user story: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task UpdateEnhancementUserStoryAsync()
    {
        try
        {
            if (_selectedEnhancementUserStoryId == null)
            {
                MessageBox.Show("Please select an enhancement user story to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new EnhancementUserStoryUpdateRequest
            {
                EnhancementID = ParseNullableInt(txtEnhancementID.Text),
                JiraIssue = string.IsNullOrWhiteSpace(txtJiraIssue.Text) ? null : txtJiraIssue.Text.Trim(),
                StoryPoints = ParseNullableInt(txtStoryPoints.Text),
                JiraIssueLink = string.IsNullOrWhiteSpace(txtJiraIssueLink.Text) ? null : txtJiraIssueLink.Text.Trim(),
                JiraIssueSummary = string.IsNullOrWhiteSpace(txtJiraIssueSummary.Text) ? null : txtJiraIssueSummary.Text.Trim(),
                StoryPointCostID = ParseNullableInt(txtStoryPointCostID.Text)
            };

            await _enhancementUserStoryApiClient.UpdateAsync(_selectedEnhancementUserStoryId.Value, request);
            MessageBox.Show("Enhancement User Story updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadEnhancementUserStoriesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating enhancement user story: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task DeleteEnhancementUserStoryAsync()
    {
        try
        {
            if (_selectedEnhancementUserStoryId == null)
            {
                MessageBox.Show("Please select an enhancement user story to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this enhancement user story?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                await _enhancementUserStoryApiClient.DeleteAsync(_selectedEnhancementUserStoryId.Value);
                MessageBox.Show("Enhancement User Story deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadEnhancementUserStoriesAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting enhancement user story: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        _selectedEnhancementUserStoryId = null;
        txtEnhancementID.Text = string.Empty;
        txtJiraIssue.Text = string.Empty;
        txtStoryPoints.Text = string.Empty;
        txtJiraIssueLink.Text = string.Empty;
        txtJiraIssueSummary.Text = string.Empty;
        txtStoryPointCostID.Text = string.Empty;
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
}
