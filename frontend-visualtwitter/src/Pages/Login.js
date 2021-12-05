
import axios from 'axios';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, FormControl, TextField, Button } from '@mui/material';

const LoginPage = () => {
    // const navigate = useNavigate();
    const [state, setState] = useState({
        "Email": "",
        "Password": ""
    })
    const handleEmailChange = (e) => {
        var email = e.target.value;
        setState({ ...state, email: email })

    }
    const handlePasswordChange = (e) => {
        var password = e.target.value;
        setState({ ...state, password: password })
    }
    // const handleSubmit = () => {
    //         axios.post("http://localhost:8000/login", { email: state.email, password: state.password }, { withCredentials: true }).then((response) => {
    //             localStorage.setItem('token', response.data.token)
    //             history.push("/dashboard")
    //         })
    // }
    return (

        <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center",minHeight:"35vh" }}>
            <FormControl >
                <TextField label="email" onChange={handleEmailChange}></TextField>
                <TextField label="password" onChange={handlePasswordChange}></TextField>
                <Button variant="contained" color="primary" /*onClick={handleSubmit}*/>Login</Button>

            </FormControl>
        </Box>

    )
}

export { LoginPage }