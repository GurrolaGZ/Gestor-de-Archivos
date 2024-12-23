﻿using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DataAccess;

namespace Presentation
{
    public partial class FormTaller : Form
    {
        private EmpleadosDao empleadosDao;  // Declarar empleadosDao

        public FormTaller()
        {
            InitializeComponent();
            empleadosDao = new EmpleadosDao();  // Inicializar empleadosDao
            CargarEmpleadosEnDataGrid("Taller");  // Cargar empleados del área "Taller"
            ConfigurarDataGridView();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CargarEmpleadosEnDataGrid(string area)
        {
            try
            {
                DataTable empleados = empleadosDao.ObtenerEmpleadosPorArea(area);  // Obtener empleados por área
                dataGridViewEmpleados.DataSource = empleados;                       // Asignar al DataGridView

                // Verificar si la columna de "Ver más detalles" ya existe para evitar duplicados
                if (!dataGridViewEmpleados.Columns.Contains("VerDetalles"))
                {
                    DataGridViewButtonColumn btnDetalles = new DataGridViewButtonColumn();
                    btnDetalles.Name = "VerDetalles";
                    btnDetalles.Text = "Ver más detalles";
                    btnDetalles.UseColumnTextForButtonValue = true; // Mostrar el texto en los botones
                    dataGridViewEmpleados.Columns.Add(btnDetalles);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar empleados: " + ex.Message);
            }
        }

        private void ConfigurarDataGridView()
        {
            // Configuraciones de estilo del DataGridView (puedes cambiar los colores si deseas que sean diferentes por área)
            dataGridViewEmpleados.BackgroundColor = Color.Black;
            dataGridViewEmpleados.BorderStyle = BorderStyle.None;
            dataGridViewEmpleados.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 0, 0);
            dataGridViewEmpleados.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewEmpleados.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);

            dataGridViewEmpleados.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 0, 0);
            dataGridViewEmpleados.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridViewEmpleados.DefaultCellStyle.BackColor = Color.FromArgb(40, 0, 0);
            dataGridViewEmpleados.DefaultCellStyle.ForeColor = Color.White;

            dataGridViewEmpleados.EnableHeadersVisualStyles = false;
            dataGridViewEmpleados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewEmpleados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewEmpleados.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        }

        private void FormTaller_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void AbrirDetalleEmpleado(int empleadoId)
        {
            // Abrir el formulario de detalles del empleado
            FormDetalleEmpleado formDetalle = new FormDetalleEmpleado(empleadoId); // Cambiar a ID de empleado
            formDetalle.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void dataGridViewEmpleados_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar si se ha hecho clic en la columna de "VerDetalles"
            if (dataGridViewEmpleados.Columns[e.ColumnIndex].Name == "VerDetalles")
            {
                // Obtener el valor de la celda "EmpleadoID"
                var empleadoIdCellValue = dataGridViewEmpleados.Rows[e.RowIndex].Cells["EmpleadoID"].Value;

                // Verificar si el valor es DBNull o null
                if (empleadoIdCellValue != DBNull.Value && empleadoIdCellValue != null)
                {
                    int empleadoId = Convert.ToInt32(empleadoIdCellValue); // Convertir a int solo si no es DBNull
                    AbrirDetalleEmpleado(empleadoId);  // Llamar al método para abrir el formulario de detalles
                }
                else
                {
                    // Manejar el caso donde el ID del empleado no es válido
                    MessageBox.Show("Seleccione un empleado valido");
                }
            }
        }
    }
}
