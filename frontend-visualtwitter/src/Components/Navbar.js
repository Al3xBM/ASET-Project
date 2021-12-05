
import React, { useContext } from 'react';
import { BrowserRouter, Link, Route, Routes } from 'react-router-dom';
import { LoginPage } from '../Pages/Login';
import { Dashboard } from '../Pages/Dashboard';
import { RegisterPage } from '../Pages/Register';
import {Typography,Button, AppBar, Toolbar} from '@mui/material'
import { useNavigate } from 'react-router-dom';
import axios from 'axios'
const Navbar=()=>{
 // const navigate=useNavigate();
//   const handleLogoutButton=()=>{
//       axios.post("http://localhost:8000/api/logout",{ withCredentials: true }).then((response) => {
//         history.push("/login")
//     })  
//   }
    return(
    <BrowserRouter>
        <AppBar position="static">
           <Toolbar variant="dense" >
           <Typography variant="h6" component="div" sx={{ flexGrow: 0.01 }}>
           <Link to="/dashboard" color="inherit" style={{ textDecoration: 'none' ,color:"white"}}>Dashboard</Link>
           
          </Typography>
          <Typography variant="h6" component="div" sx={{ flexGrow: 0.01}}>
          <Link to="/login" color="primary" style={{ textDecoration: 'none',color:"white"  }}>Login</Link>
           
          </Typography>
          <Typography variant="h6" component="div" sx={{ flexGrow: 0.01}}>
          <Link to="/register" color="primary" style={{ textDecoration: 'none',color:"white"  }}>Register</Link>
           
          </Typography>
          
           </Toolbar>
   
           
         </AppBar>
         <Routes>
         <Route path="/dashboard" element={<Dashboard />} />
         <Route path="/login" element={<LoginPage />} />
         <Route path="/register" element={< RegisterPage />} />
         </Routes>
       </BrowserRouter>)
}
export {Navbar};