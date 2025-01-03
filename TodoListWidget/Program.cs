using System;
using System.Drawing;
using System.Windows.Forms;

namespace ToDoListWidget
{
    public class ToDoForm : Form
    {
        private ListBox toDoList;
        private TextBox inputBox;
        private Button addButton;
        private Button removeButton;
        private NotifyIcon trayIcon;

        public ToDoForm()
        {
            this.Text = "To-Do List";
            this.Size = new Size(300, 400);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.TopMost = true;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width - 10, 10);

            toDoList = new ListBox() { Top = 10, Left = 10, Width = 260, Height = 250 };
            inputBox = new TextBox() { Top = 270, Left = 10, Width = 180 };
            addButton = new Button() { Text = "Add", Top = 270, Left = 200, Width = 70 };
            removeButton = new Button() { Text = "Remove", Top = 310, Left = 10, Width = 260 };

            addButton.Click += AddButton_Click;
            removeButton.Click += RemoveButton_Click;

            this.Controls.Add(toDoList);
            this.Controls.Add(inputBox);
            this.Controls.Add(addButton);
            this.Controls.Add(removeButton);

            trayIcon = new NotifyIcon();
            trayIcon.Icon = SystemIcons.Application;
            trayIcon.Text = "To-Do List";
            trayIcon.Visible = true;

            ContextMenu trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Show", (s, e) => this.Show());
            trayMenu.MenuItems.Add("Hide", (s, e) => this.Hide());
            trayMenu.MenuItems.Add("Exit", (s, e) => Application.Exit());

            trayIcon.ContextMenu = trayMenu;

            this.Shown += (s, e) => this.Hide();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string task = inputBox.Text.Trim();
            if (!string.IsNullOrEmpty(task))
            {
                toDoList.Items.Add(task);
                inputBox.Clear();
            }
            else
            {
                MessageBox.Show("Please enter a task.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (toDoList.SelectedItem != null)
            {
                toDoList.Items.Remove(toDoList.SelectedItem);
            }
            else
            {
                MessageBox.Show("Please select a task to remove.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
