using RpaAlmClient.Forms;

namespace RpaAlmClient.Forms;

public partial class MainMenuForm : Form
{
    public MainMenuForm()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        this.Text = "RPA ALM Client - Main Menu";
        this.Size = new Size(720, 420);
        this.StartPosition = FormStartPosition.CenterScreen;

        var label = new Label
        {
            Text = "RPA Application Lifecycle Management",
            Font = new Font("Arial", 16, FontStyle.Bold),
            Location = new Point(150, 20),
            Size = new Size(420, 30),
            TextAlign = ContentAlignment.MiddleCenter
        };

        // Lookup Tables Section
        var lblLookupTables = new Label
        {
            Text = "Reference Data Management:",
            Font = new Font("Arial", 12, FontStyle.Bold),
            Location = new Point(50, 70),
            Size = new Size(300, 25)
        };

        int yPos = 100;
        CreateMenuButton("Status", () => new StatusManagementForm().Show(), new Point(50, yPos));
        CreateMenuButton("Complexity", () => new ComplexityManagementForm().Show(), new Point(240, yPos));
        CreateMenuButton("Medal", () => new MedalManagementForm().Show(), new Point(430, yPos));

        yPos += 40;
        CreateMenuButton("Region", () => new RegionManagementForm().Show(), new Point(50, yPos));
        CreateMenuButton("Segment", () => new SegmentManagementForm().Show(), new Point(240, yPos));
        CreateMenuButton("Function", () => new FunctionManagementForm().Show(), new Point(430, yPos));

        yPos += 40;
        CreateMenuButton("SLA Item Type", () => new SlaItemTypeManagementForm().Show(), new Point(50, yPos));
        CreateMenuButton("Environment Type", () => new AutomationEnvironmentTypeManagementForm().Show(), new Point(240, yPos));
        CreateMenuButton("AD Domain", () => new ADDomainManagementForm().Show(), new Point(430, yPos));

        yPos += 40;
        CreateMenuButton("Enhancement", () => new EnhancementManagementForm().Show(), new Point(50, yPos));
        CreateMenuButton("Story Point Cost", () => new StoryPointCostManagementForm().Show(), new Point(240, yPos));

        // Note about future features
        yPos += 60;
        var lblNote = new Label
        {
            Text = "Core entity forms (Automation, SLA, Virtual Identity, etc.) coming soon...",
            Font = new Font("Arial", 9, FontStyle.Italic),
            ForeColor = System.Drawing.Color.Gray,
            Location = new Point(50, yPos),
            Size = new Size(600, 20)
        };

        this.Controls.Add(label);
        this.Controls.Add(lblLookupTables);
        this.Controls.Add(lblNote);
    }

    private void CreateMenuButton(string text, Action onClick, Point location)
    {
        var button = new Button
        {
            Text = text,
            Location = location,
            Size = new Size(180, 30),
            Font = new Font("Arial", 9)
        };
        button.Click += (s, e) => onClick();
        this.Controls.Add(button);
    }
}
