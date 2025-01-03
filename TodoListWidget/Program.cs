using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ToDoListWidget
{
    public class ToDoForm : Form
    {
        private FlowLayoutPanel taskPanel;
        private TextBox inputBox;
        private Button addButton;
        private NotifyIcon trayIcon;

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        private const int HWND_MESSAGE = -3;

        public ToDoForm()
        {
            this.Text = "To-Do List";
            this.Size = new Size(300, 200);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.TopMost = false;
            this.ShowInTaskbar = false;
            this.BackColor = Color.LightBlue;
            this.Region = new Region(GetRoundedRectanglePath(new Rectangle(0, 0, this.Width, this.Height), 40));
            this.MinimumSize = new Size(150, 200);

            taskPanel = new FlowLayoutPanel()
            {
                Top = 10,
                Left = 10,
                Width = 280,
                Height = 130,
                AutoScroll = true,
                BackColor = Color.Transparent
            };

            inputBox = new TextBox() { Top = 150, Left = 10, Width = 200, BorderStyle = BorderStyle.FixedSingle };
            inputBox.KeyDown += InputBox_KeyDown;

            addButton = new Button() { Text = "Add", Top = 150, Left = 220, Width = 50, FlatStyle = FlatStyle.Flat };

            addButton.Click += AddButton_Click;

            this.Controls.Add(taskPanel);
            this.Controls.Add(inputBox);
            this.Controls.Add(addButton);

            trayIcon = new NotifyIcon();
            trayIcon.Icon = SystemIcons.Application;
            trayIcon.Text = "To-Do List";
            trayIcon.Visible = true;

            ContextMenu trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Show", (s, e) => {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.BringToFront();
            });
            trayMenu.MenuItems.Add("Hide", (s, e) => this.Hide());
            trayMenu.MenuItems.Add("Exit", (s, e) => {
                trayIcon.Visible = false;
                Application.Exit();
            });

            trayIcon.ContextMenu = trayMenu;

            this.FormClosing += (s, e) => {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    e.Cancel = true;
                    this.Hide();
                }
            };

            MoveToDesktop();

            this.Resize += (s, e) =>
            {
                taskPanel.Width = this.ClientSize.Width - 20;
                taskPanel.Height = this.ClientSize.Height - 70;
                inputBox.Width = this.ClientSize.Width - 80;
                addButton.Left = this.ClientSize.Width - 60;
                addButton.Top = this.ClientSize.Height - 40;
                inputBox.Top = this.ClientSize.Height - 40;
            };

            this.MouseDown += Form_MouseDown;
            this.MouseMove += Form_MouseMove;

            this.Shown += (s, e) => this.Show();
        }

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            dragging = false;
            base.OnMouseUp(e);
        }

        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rect.Left, rect.Top, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Top, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void MoveToDesktop()
        {
            IntPtr desktopHandle = FindWindow("Progman", null);
            if (desktopHandle != IntPtr.Zero)
            {
                SetParent(this.Handle, desktopHandle);
            }
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddTask();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            AddTask();
        }

        private void AddTask()
        {
            string task = inputBox.Text.Trim();
            if (!string.IsNullOrEmpty(task))
            {
                Label taskLabel = new Label()
                {
                    Text = "• " + task,
                    AutoSize = true,
                    Font = new Font("Arial", 12, FontStyle.Regular),
                    ForeColor = Color.Black
                };

                CheckBox checkBox = new CheckBox()
                {
                    Checked = false,
                    AutoSize = true
                };

                checkBox.CheckedChanged += (s, ev) =>
                {
                    taskLabel.Font = new Font(taskLabel.Font, checkBox.Checked ? FontStyle.Strikeout : FontStyle.Regular);
                };

                taskLabel.Click += (s, ev) =>
                {
                    taskPanel.Controls.Remove((Control)taskLabel.Parent);
                };

                FlowLayoutPanel taskContainer = new FlowLayoutPanel()
                {
                    Width = taskPanel.Width - 20,
                    Height = 30,
                    BackColor = Color.Transparent
                };

                taskContainer.Controls.Add(checkBox);
                taskContainer.Controls.Add(taskLabel);
                taskPanel.Controls.Add(taskContainer);

                inputBox.Clear();
            }
            else
            {
                MessageBox.Show("Please enter a task.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ToDoForm());
        }
    }
}
