import { FormControl, Box, TextField, Button } from '@mui/material';

import React, { useContext, useState } from 'react';
import { SearchResultTemplate1 } from '../Components/SearchResultTemplate1';
import axios from 'axios'
import { SearchResultTemplate1Context } from '../Contexts/SearchResultTemplate1Context';

const Dashboard = () => {
    const [event, setEvent] = useState({
        'event': ""
    })
    const { setSearchResult } = useContext(SearchResultTemplate1Context);
    const handleSubmit = (e) => {
        e.preventDefault();
        console.log("event.event: ",event.event)
        axios.post("http://localhost:5000/clusterized", { "topic" : event.event}).then((response) => {
               setSearchResult(response.data);
            })
        //setShowSearchResult(true);
    }
    
    const [showSearchResult,setShowSearchResult] = useState(false);

    return (

        <Box sx={{ display: "flex",flexDirection:"column", alignItems: "center", justifyContent: "center", minHeight: "35vh" }}>
            <FormControl>
                <Box sx={{ display: "flex", direction: "inline" }}>
                    <TextField label="Search BasketBall event " value={event.event} required onChange={(e) => {setEvent({ ...event, event: e.target.value }) }} />
                    <Button onClick={handleSubmit} variant="contained" color="primary">Search</Button>
                </Box>
            </FormControl>
            <Box sx = {{display:"flex",  justifyContent: "center",flexDirection:"inline",width:"600px"}}>
            
            
            </Box>
            <SearchResultTemplate1>

            </SearchResultTemplate1>
        </Box>





    )
}
export { Dashboard };