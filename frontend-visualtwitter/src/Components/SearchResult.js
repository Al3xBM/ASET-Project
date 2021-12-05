import { FormLabel,Box } from '@mui/material';

import React from 'react';


const SearchResult=()=>
{
    return(
    <>
        <Box sx={{display:"flex", flexDirection:"column", border:"0.5px solid grey",width:"400px"}}>
            <FormLabel>Team1: </FormLabel>
            <FormLabel>Team2:</FormLabel>
            <FormLabel>Team1Score:</FormLabel>
            <FormLabel>Team2Score:</FormLabel>
            <FormLabel>Team1MVP:</FormLabel>
            <FormLabel>Team2MVP:</FormLabel>
            <FormLabel>OtherInfo:</FormLabel>
        </Box>
        <Box sx={{display:"flex", flexDirection:"column", border:"0.5px solid grey",width:"400px"}}>
            <FormLabel>RelatedTweets:</FormLabel>
        </Box>

   </>
    )
}

export { SearchResult };