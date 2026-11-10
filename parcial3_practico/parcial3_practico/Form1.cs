using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using BCrypt.Net;

namespace parcial3_practico
{
    public partial class Form1 : Form
    {
        // Cadena de conexión (va aquí arriba, dentro de la clase)
        private const string cs = "Server=localhost;Port=3306;Database=LoginDB;Uid=root;Pwd=alizn03;";

        public Form1()
        {
            InitializeComponent();
            this.Text = "Inicio de Sesión";
            this.btnAcceder.Click += new EventHandler(this.btnAcceder_Click);
            this.btnRegistrar.Click += new EventHandler(this.btnRegistrar_Click);
            this.btnSalir.Click += new EventHandler(this.btnSalir_Click);
        }

        //  ACCEDER 
        private void btnAcceder_Click(object? sender, EventArgs? e)
        {
            errorProvider1.Clear();
            bool hayError = false;

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                errorProvider1.SetError(txtUsuario, "Ingrese su nombre de usuario.");
                hayError = true;
            }

            if (string.IsNullOrWhiteSpace(txtContraseña.Text))
            {
                errorProvider1.SetError(txtContraseña, "Ingrese su contraseña.");
                hayError = true;
            }

            if (hayError) return;

            try
            {
                using var con = new MySqlConnection(cs);
                con.Open();

                var cmd = new MySqlCommand("SELECT PasswordHash FROM Usuarios WHERE NombreUsuario = @u", con);
                cmd.Parameters.AddWithValue("@u", txtUsuario.Text.Trim());

                bool exito = false;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string hashGuardado = reader.GetString(0).Trim();
                        exito = BCrypt.Net.BCrypt.Verify(txtContraseña.Text, hashGuardado);
                    }
                }

                var log = new MySqlCommand("INSERT INTO IntentosLogin (NombreUsuario, Exitoso, Fecha) VALUES (@u, @e, NOW())", con);
                log.Parameters.AddWithValue("@u", txtUsuario.Text.Trim());
                log.Parameters.AddWithValue("@e", exito);
                log.ExecuteNonQuery();

                if (exito)
                {
                    MessageBox.Show($"¡Bienvenido, {txtUsuario.Text.Trim()}!", "Acceso concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtContraseña.Clear();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // REGISTRAR 
        private void btnRegistrar_Click(object? sender, EventArgs? e)
        {
            errorProvider1.Clear();
            bool hayError = false;

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                errorProvider1.SetError(txtUsuario, "Ingrese un nombre de usuario.");
                hayError = true;
            }

            if (string.IsNullOrWhiteSpace(txtContraseña.Text))
            {
                errorProvider1.SetError(txtContraseña, "Ingrese una contraseña.");
                hayError = true;
            }

            if (hayError) return;

            // Se genera el hash con BCrypt
            string hash = BCrypt.Net.BCrypt.HashPassword(txtContraseña.Text);

            try
            {
                using var con = new MySqlConnection(cs);
                con.Open();

                var cmd = new MySqlCommand(
                    "INSERT INTO Usuarios (NombreUsuario, PasswordHash) VALUES (@u, @h)", con);
                cmd.Parameters.AddWithValue("@u", txtUsuario.Text.Trim());
                cmd.Parameters.AddWithValue("@h", hash);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Usuario registrado correctamente.", "Registro",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtContraseña.Clear();
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                MessageBox.Show("Ese usuario ya existe.", "Registro",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // SALIR
        private void btnSalir_Click(object? sender, EventArgs? e)
        {
            Application.Exit();
        }
    }
}