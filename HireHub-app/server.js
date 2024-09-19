const express = require('express');
const sql = require('mssql');
const dbConfig = {
    user: 'Angel02w_SQLLogin_1',
    password: 'wf54z3ymxg',
    server: 'HireHubDB.mssql.somee.com',
    database: 'HireHubDB',
    options: {
        encrypt: true, // Necesario para Azure SQL
        enableArithAbort: true,
        trustServerCertificate: true
    }
};

const app = express();
const port = 3000;

app.use(express.json());

// Conexión a la base de datos
async function connectToDB() {
    try {
        await sql.connect(dbConfig);
        console.log('Connected to the database');
    } catch (err) {
        console.error('Database connection failed', err);
    }
}

