import React, {useState} from 'react';
import {useNavigate} from 'react-router-dom';
import {Box, TextField, FormControl, Button} from '@mui/material'
import axios from 'axios'

const RegisterPage= () =>
{
    const navigate = useNavigate();
    const [state, setState] = useState({
        "firstName": "",
        "lastName": "",
        "email": "",
        "password": ""
    })

    const handleFirstNameChange = (e) => {
        var name = e.target.value;
        setState({ ...state, firstName: name })

    }
    const handleLastNameChange = (e) => {
        var name = e.target.value;
        setState({ ...state, lastName: name })

    }
    const handleEmailChange = (e) => {
        var email = e.target.value;
        setState({ ...state, email: email })

    }
    const handlePasswordChange = (e) => {
        var password = e.target.value;
        setState({ ...state, password: password })

    }
    const handleSubmit = () => {
            axios.post("http://localhost:5000/api/v1/users/register", { "FirstName": state.firstName, "LastName": state.lastName, "Email": state.email, "Password": state.password }).then(() => {
                navigate("/login")
            })
    }
    return (
        <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center",minHeight:"35vh" }}>
            <FormControl align="center">
                <TextField label="First Name" onChange={handleFirstNameChange} />
                <TextField label="Last Name" onChange={handleLastNameChange} />
                <TextField label="Email" onChange={handleEmailChange} />
                <TextField label="Password" onChange={handlePasswordChange} />
                <Button variant="contained" color="primary" onClick={handleSubmit}>Register</Button>
            </FormControl>
        </Box>

    )
}
export { RegisterPage };