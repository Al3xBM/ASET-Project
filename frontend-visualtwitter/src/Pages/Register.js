import React, {useState} from 'react';
import {useNavigate} from 'react-router-dom';
import {Box, TextField, FormControl, Button} from '@mui/material'

const RegisterPage= () =>
{
 //   const navigate = useNavigate();
    const [state, setState] = useState({
        "name": "",
        "email": "",
        "password": ""
    })

    const handleNameChange = (e) => {
        var name = e.target.value;
        setState({ ...state, name: name })

    }
    const handleEmailChange = (e) => {
        var email = e.target.value;
        setState({ ...state, email: email })

    }
    const handlePasswordChange = (e) => {
        var password = e.target.value;
        setState({ ...state, password: password })

    }
    // const handleSubmit = () => {
    //         axios.post("http://localhost:8000/register", { name: state.name, email: state.email, password: state.password }, { withCredentials: true }).then(() => {
    //             history.push("/login")
    //         })
    // }
    return (
        <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center",minHeight:"35vh" }}>
            <FormControl align="center">
                <TextField label="Name" onChange={handleNameChange} />
                <TextField label="Email" onChange={handleEmailChange} />
                <TextField label="Password" onChange={handlePasswordChange} />
                <Button variant="contained" color="primary" /*onClick={handleSubmit}*/>Register</Button>
            </FormControl>
        </Box>

    )
}
export { RegisterPage };