using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Acceso_a_datos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            txtFecha.ReadOnly = true;
            rdbMasculino.Checked = true;
            rdbSoltero.Checked = true;
        }

        private void bunifuDatepicker1_onValueChanged(object sender, EventArgs e)
        {
            DateTime dateTime = datePicker.Value;
            txtFecha.Text = datePicker.Value.ToString("dd-MM -yyy");
        }

        private void txtCelular_OnValueChanged(object sender, EventArgs e)
        {
            if (txtCelular.Text.Length == 3)
                txtCelular.Text += "-";
            if (txtCelular.Text.Length == 7)
                txtCelular.Text += "-";
        }

        private void txtTelefono_OnValueChanged(object sender, EventArgs e)
        {
            if (txtTelefono.Text.Length == 3)
                txtTelefono.Text += "-";
            if (txtTelefono.Text.Length == 7)
                txtTelefono.Text += "-";
        }
        bool check()
        {
            bool condicion=true;
            string cadena="";
            if (txtNombre.Text == string.Empty)
            {
                condicion = false;
                cadena += "Nombre-";
            }
            if (txtApellido.Text == string.Empty)
            {
                condicion = false;
                cadena += "Apellido-";
            }
            if (txtFecha.Text == string.Empty)
            {
                condicion = false;
                cadena += "Fecha-";
            }
            if (txtDireccion.Text == string.Empty)
            {
                condicion = false;
                cadena += "Direccion-";
            }
            if (txtTelefono.Text == string.Empty)
            {
                condicion = false;
                cadena += "Telefono-";
            }
            if (txtCelular.Text == string.Empty)
            {
                condicion = false;
                cadena += "Celular-";
            }
            if (txtCorreo.Text == string.Empty)
            {
                condicion = false;
                cadena += "Correo-";
            }
            
            return condicion;
        }
        //conexion
        SqlConnection con = new SqlConnection("Server=DESKTOP-949EGD7 ;DataBase=agenda; Integrated Security=true");
        //abrir conexion
        private SqlConnection AbrirConexion()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            return con;
        }
        //cerrar conexion
        private SqlConnection CerrarConexion()
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            return con;
        }
        //CRUD-create
        void create(String nombre, String apellido, DateTime fecha, String direccion, String genero,String estado_civil, String telefono_celular, String telefono, String correo)
        {
            try
            {
                AbrirConexion();
                //verificar si ese registro ya existe
                SqlCommand cmd = new SqlCommand("select @nombre,@apellido from lista where nombre=@nombre and apellido =@apellido", con);
                cmd.Parameters.AddWithValue("nombre", nombre);
                cmd.Parameters.AddWithValue("apellido", apellido);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    MessageBox.Show("Esa persona ya esta registrada");
                }
                else
                {
                    cmd = new SqlCommand("insert into lista VALUES(@nombre,@apellido,@fecha_nacimiento,@direccion,@genero,@estado_civil,@telefono_celular,@telefono,@Correo_electronico)", con);
                    cmd.Parameters.AddWithValue("nombre", nombre);
                    cmd.Parameters.AddWithValue("apellido", apellido);
                    cmd.Parameters.AddWithValue("fecha_nacimiento", fecha);
                    cmd.Parameters.AddWithValue("direccion", direccion);
                    cmd.Parameters.AddWithValue("genero", genero);
                    cmd.Parameters.AddWithValue("estado_civil", estado_civil);
                    cmd.Parameters.AddWithValue("telefono_celular", telefono_celular);
                    cmd.Parameters.AddWithValue("telefono", telefono);
                    cmd.Parameters.AddWithValue("Correo_electronico", correo);
                    sda = new SqlDataAdapter(cmd);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("registro guardado con exito");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                CerrarConexion();
            }
        }


        //CRUD-read
        void read(String nombre, String apellido)
        {
            try
            {
                String genero="";
                String estadoCivil="";
                AbrirConexion();
                SqlCommand cmd = new SqlCommand("select nombre,apellido,fecha_nacimiento, direccion, genero, estado_civil, telefono_celular, telefono, Correo_electronico from lista where nombre = @nombre and apellido = @apellido ",con);
                cmd.Parameters.AddWithValue("nombre", nombre);
                cmd.Parameters.AddWithValue("apellido", apellido);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtNombre.Text = reader["nombre"].ToString();
                    txtApellido.Text = reader["apellido"].ToString();
                    datePicker.Value = DateTime.Parse(reader["fecha_nacimiento"].ToString());
                    txtDireccion.Text = reader["direccion"].ToString();
                    genero = reader["genero"].ToString();
                    estadoCivil = reader["estado_civil"].ToString();
                    txtCelular.Text = reader["telefono_celular"].ToString();
                    txtTelefono.Text = reader["telefono"].ToString();
                    txtCorreo.Text = reader["Correo_electronico"].ToString();
                    if (genero == rdbMasculino.Text)
                    {
                        rdbMasculino.Checked = true;
                    }
                    else
                    {
                        rdbFemenino.Checked = true;
                    }
                    if (estadoCivil == rdbSoltero.Text)
                    {
                        rdbSoltero.Checked = true;
                    }
                    else if (estadoCivil == rdbCasado.Text)
                    {
                        rdbCasado.Checked = true;
                    }
                    else if (estadoCivil == rdbDivorciado.Text)
                    {
                        rdbDivorciado.Checked = true;
                    }
                    else if (estadoCivil == rdbViudo.Text)
                    {
                        rdbViudo.Checked = true;
                    }

                }
                else
                {
                    MessageBox.Show("Registro no encontrado");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                CerrarConexion();
            }
        }
        //CRUD-update
        void update(String nombre, String apellido, DateTime fecha, String direccion, String genero, String estado_civil, String telefono_celular, String telefono, String correo)
        {
            try 
            {
                AbrirConexion();
                SqlCommand cmd = new SqlCommand("update lista set nombre=@nombre,apellido=@apellido,fecha_nacimiento=@fecha_nacimiento,direccion=@direccion,genero=@genero,estado_civil=@estado_civil,telefono_celular=@telefono_celular,telefono=@telefono,Correo_electronico=@Correo_electronico where nombre=@nombre and apellido=@apellido", con);
                cmd.Parameters.AddWithValue("nombre", nombre);
                cmd.Parameters.AddWithValue("apellido", apellido);
                cmd.Parameters.AddWithValue("fecha_nacimiento", fecha);
                cmd.Parameters.AddWithValue("direccion", direccion);
                cmd.Parameters.AddWithValue("genero", genero);
                cmd.Parameters.AddWithValue("estado_civil", estado_civil);
                cmd.Parameters.AddWithValue("telefono_celular", telefono_celular);
                cmd.Parameters.AddWithValue("telefono", telefono);
                cmd.Parameters.AddWithValue("Correo_electronico", correo);
                cmd.ExecuteNonQuery();
                MessageBox.Show("registro actualizado con exito");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                CerrarConexion();
            }
        }
        //CRUD-delete
        void delete(String nombre, String apellido)
        {
            try
            {
                AbrirConexion();
                SqlCommand cmd = new SqlCommand("delete from lista where nombre=@nombre and apellido=@apellido", con);
                cmd.Parameters.AddWithValue("nombre", nombre);
                cmd.Parameters.AddWithValue("apellido", apellido);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Registro eliminado con exito", "Aviso");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                CerrarConexion();
            }
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            String sexo;
            if (rdbMasculino.Checked == true)
            {
                sexo = rdbMasculino.Text;
            }
            else
            {
                sexo = rdbFemenino.Text;
            }
            String estadoCivil = " ";
            if (rdbSoltero.Checked == true)
            {
                estadoCivil = rdbSoltero.Text;
            }
            else if (rdbCasado.Checked == true)
            {
                estadoCivil = rdbCasado.Text;
            }
            else if (rdbDivorciado.Checked == true)
            {
                estadoCivil = rdbDivorciado.Text;
            }
            else if (rdbViudo.Checked == true)
            {
                estadoCivil = rdbViudo.Text;
            }

            create(txtNombre.Text, txtApellido.Text, datePicker.Value, txtDireccion.Text, sexo, estadoCivil, txtCelular.Text, txtTelefono.Text, txtCorreo.Text);

        }


        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (check() == false)
            {
                btnGuardar.Enabled = false;
                btnActualizar.Enabled = false;
            }
            if (check() == true)
            {
                btnGuardar.Enabled = true;
                btnActualizar.Enabled = true;
            }
            if (txtNombre.Text == String.Empty || txtApellido.Text == String.Empty)
            {
                btnBuscar.Enabled = false;
                btnBorrar.Enabled = false;
            }
            else
            {
                btnBuscar.Enabled = true;
                btnBorrar.Enabled = true;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            read(txtNombre.Text, txtApellido.Text);
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("seguro que quiere realizar esta accion","Aviso",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2)== DialogResult.Yes)
            {
                delete(txtNombre.Text, txtApellido.Text);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            String sexo;
            if (rdbMasculino.Checked == true)
            {
                sexo = rdbMasculino.Text;
            }
            else
            {
                sexo = rdbFemenino.Text;
            }
            String estadoCivil = " ";
            if (rdbSoltero.Checked == true)
            {
                estadoCivil = rdbSoltero.Text;
            }
            else if (rdbCasado.Checked == true)
            {
                estadoCivil = rdbCasado.Text;
            }
            else if (rdbDivorciado.Checked == true)
            {
                estadoCivil = rdbDivorciado.Text;
            }
            else if (rdbViudo.Checked == true)
            {
                estadoCivil = rdbViudo.Text;
            }

            update(txtNombre.Text, txtApellido.Text, datePicker.Value, txtDireccion.Text, sexo, estadoCivil, txtCelular.Text, txtTelefono.Text, txtCorreo.Text);
        }
    }
}
