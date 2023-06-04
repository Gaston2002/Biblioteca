using BiblotecaDb;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Biblioteca
{
    public partial class Form1 : Form
    {
        //creacion de listas para libros
        DataTable dtLibros = new DataTable() { TableName = "libros" };

        //mensajes de error
        const string ERROR_CODIGO_EXISTE = "Código existente, ingrese uno nuevo.";
        const string ERROR_CODIGO_BORRADO = "Libro no seleccionado";
        const string ERROR_CODIGO_PRESTADO = "Libro no seleccionado";
        const string ERROR_LIBRO_PRESTADO = "El libro no se haya disponible";
        const string DIRECCION_XML = @"C:\Users\Usuario\OneDrive\Escritorio\FACU\PROGRAMACIÓN\vs2022\Gestor_Libros\";

        string estado;

        public Form1()
        {
            InitializeComponent();
            //columnas de mi tabla
            dtLibros.Columns.Add("Código");
            dtLibros.Columns.Add("Título");
            dtLibros.Columns.Add("Autor");
            dtLibros.Columns.Add("Editorial");
            dtLibros.Columns.Add("Género");
            dtLibros.Columns.Add("Estado");

            //lectura y carga de datos del dto
            Leer_DT();
            dataTable.DataSource = dtLibros;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //creacion de nuevo libro con sus atributos
            Libro libro = new Libro();

            int indice = cmbGenero.SelectedIndex;

            libro.Codigo = txtCode.Text;
            libro.Titulo = txtTitle.Text;
            libro.Autor = txtAuthor.Text;
            libro.Editorial = txtEditorial.Text;

            //limpieza del error provider
            epvTextos.Clear();

            ////validacion para llenar los campos
            if (Validar())
            {

            }
            else
            {
                libro.Genero = cmbGenero.Items[indice].ToString();
                //validacion de codigo existente
                int fila = BuscarCodigo(libro.Codigo);
                if (fila != -1)
                {
                    MessageBox.Show(ERROR_CODIGO_EXISTE);
                }
                else
                {
                    estado = "Disponible";

                    //agregamos el nuevo libro a la lista
                    dtLibros.Rows.Add(new object[] { libro.Codigo, libro.Titulo, libro.Autor, libro.Editorial, libro.Genero, estado });
                    //dtLibros.WriteXml(DIRECCION_XML + "librito.xml");

                    //limpieza de txts y cmb
                    txtCode.Clear();
                    txtTitle.Clear();
                    txtAuthor.Clear();
                    txtEditorial.Clear();
                    cmbGenero.SelectedIndex = -1;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //validacion de codigo no encontrado
            if (dataTable.CurrentRow == null)
            {
                MessageBox.Show(ERROR_CODIGO_BORRADO);
            }
            else
            {
                int fila = BuscarCodigo(dataTable.CurrentRow.Cells[0].Value.ToString());

                if (fila != -1)
                {
                    dtLibros.Rows[fila].Delete();
                    //dtLibros.WriteXml(DIRECCION_XML + "librito.xml");
                }
            }
        }

        private bool Validar()
        {
            bool validar = false;

            if (txtCode.Text == "")
            {
                epvTextos.SetError(txtCode, "llenar campo");
                validar = true;
            }
            if (txtTitle.Text == "")
            {
                epvTextos.SetError(txtTitle, "llenar campo");
                validar = true;
            }
            if (txtAuthor.Text == "")
            {
                epvTextos.SetError(txtAuthor, "llenar campo");
                validar = true;
            }
            if (txtEditorial.Text == "")
            {
                epvTextos.SetError(txtEditorial, "llenar campo");
                validar = true;
            }
            if (cmbGenero.SelectedIndex == -1)
            {
                epvTextos.SetError(cmbGenero, "llenar campo");
                validar = true;
            }
            return validar;
        }


        public int BuscarCodigo(string code)
        {
            int fila = -1;

            for (int i = 0; i < dtLibros.Rows.Count; i++)
            {
                if (dtLibros.Rows[i]["Código"].ToString() == code)
                {
                    fila = i;
                    break;
                }
            }

            return fila;
        }

        private void Leer_DT()
        {
            if (System.IO.File.Exists(DIRECCION_XML + "librito.xml"))
            {
                dtLibros.ReadXml(DIRECCION_XML + "librito.xml");
            }
        }
    }
}
