﻿using DataAccess.MySQL;
using System;
using System.Data;
using System.Windows.Forms;

namespace Presentation
{
    public partial class FormAdministrativo : Form
    {
        private FormCarpetasEmpleado formCarpetasEmpleado; // Variable para la instancia de FormCarpetasEmpleado
        private bool eventoActivo = true; // Bandera para controlar el estado del evento

        public FormAdministrativo()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized; // Minimizar la ventana
        }

        private void FormAdministrativo_Load(object sender, EventArgs e)
        {
            // Llamar al método para cargar empleados del área "Administrativo"
            CargarEmpleadosAdministrativo();
            ConfigurarDataGridView();
            PersonalizarDataGridView();
            this.dataGridViewEmpleados.CellContentClick += dataGridViewEmpleados_CellContentClick;
        }

        // Método para cargar los empleados del área "Administrativo"
        private void CargarEmpleadosAdministrativo()
        {
            try
            {
                // Crear una instancia del DAO
                EmpleadosInyeccionDao dao = new EmpleadosInyeccionDao();

                // Llamar al método para obtener empleados del área "Administrativo"
                DataTable empleados = dao.ObtenerEmpleadosPorArea("Administrativo");

                // Asignar el resultado al DataGridView
                dataGridViewEmpleados.DataSource = empleados;
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje si ocurre algún error
                MessageBox.Show("Error al cargar los empleados del área Administrativo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Configurar el DataGridView para incluir la columna de botón
        private void ConfigurarDataGridView()
        {
            DataGridViewButtonColumn detallesButtonColumn = new DataGridViewButtonColumn
            {
                Name = "Detalles",
                HeaderText = "Ver Más Detalles",
                Text = "Ver Detalles",
                UseColumnTextForButtonValue = true
            };

            // Agregar la columna al DataGridView solo si no está ya agregada
            if (dataGridViewEmpleados.Columns["Detalles"] == null)
            {
                dataGridViewEmpleados.Columns.Add(detallesButtonColumn);
            }
        }

        // Evento para manejar el clic en el botón "Ver Más Detalles"
        private void dataGridViewEmpleados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!eventoActivo) return; // Evitar que el evento se ejecute si no está activo
            eventoActivo = false; // Desactivar el evento mientras se maneja el clic

            // Verifica si la columna es la de botones
            if (e.ColumnIndex == dataGridViewEmpleados.Columns["Detalles"].Index && e.RowIndex >= 0)
            {
                // Verifica si la celda que contiene el ID del empleado está vacía o es inválida
                if (dataGridViewEmpleados.Rows[e.RowIndex].Cells["EmpleadoID"].Value == null ||
                    dataGridViewEmpleados.Rows[e.RowIndex].Cells["EmpleadoID"].Value.ToString() == string.Empty)
                {
                    // Mostrar un mensaje si no se ha seleccionado un empleado válido
                    MessageBox.Show("Por favor, seleccione un empleado válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    // Obtén el ID del empleado desde la fila seleccionada
                    int empleadoId = Convert.ToInt32(dataGridViewEmpleados.Rows[e.RowIndex].Cells["EmpleadoID"].Value);

                    // Verificar si el formulario ya está abierto
                    if (formCarpetasEmpleado == null || formCarpetasEmpleado.IsDisposed)
                    {
                        formCarpetasEmpleado = new FormCarpetasEmpleado(empleadoId);
                        formCarpetasEmpleado.Show();
                    }
                    else
                    {
                        // Traer al frente la instancia existente
                        formCarpetasEmpleado.BringToFront();
                    }
                }
            }

            // Volver a activar el evento después de un pequeño retraso
            eventoActivo = true;
        }
        // Método para personalizar el DataGridView con el diseño azul
        private void PersonalizarDataGridView()
        {
            // Personaliza el DataGridView
            dataGridViewEmpleados.AllowUserToAddRows = false;
            dataGridViewEmpleados.ReadOnly = true;
            dataGridViewEmpleados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Estilo de encabezados
            dataGridViewEmpleados.EnableHeadersVisualStyles = false;
            dataGridViewEmpleados.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dataGridViewEmpleados.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewEmpleados.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);

            // Estilo de filas
            dataGridViewEmpleados.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridViewEmpleados.RowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridViewEmpleados.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 153, 204);
            dataGridViewEmpleados.RowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Estilo de filas alternas
            dataGridViewEmpleados.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dataGridViewEmpleados.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // Bordes de celda
            dataGridViewEmpleados.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewEmpleados.BorderStyle = BorderStyle.Fixed3D;
        }
    }
}