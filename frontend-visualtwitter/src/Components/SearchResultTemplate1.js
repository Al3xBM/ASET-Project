import { FormLabel, Box, Chip, List, ListItem, Typography, ListItemText, Divider } from '@mui/material';

import React, { useContext, useEffect } from 'react';
import { SearchResultTemplate1Context } from '../Contexts/SearchResultTemplate1Context';


const SearchResultTemplate1 = () => {
  const { searchResult } = useContext(SearchResultTemplate1Context);
  console.log("searchResult: ", searchResult);
  var result = <div />
  if (searchResult !== null) {
    result =
      <List
        sx={{
          width: '100%',
          maxWidth: 360,
          bgcolor: 'background.paper',
          marginTop: '30px',
          border: 1,
          borderRadius: "16px",
          borderColor: "grey.500"
        }}
      >

        <ListItem>
          <ListItemText primary="Team 1" secondary={searchResult.team1} />
        </ListItem>
        <ListItem>
          <ListItemText primary="Team 2" secondary={searchResult.team2} />
        </ListItem>
        <Divider component="li" />
        <ListItem>
          <ListItemText primary="Team 1 score" secondary={searchResult.team1Score} />
        </ListItem>
        <ListItem>
          <ListItemText primary="Team 2 score" secondary={searchResult.team2Score} />
        </ListItem>
        <Divider component="li" />
        <ListItem>
          <ListItemText primary="Team 1 MVP" secondary={"Name: " + searchResult.team1MVP.name + "; Points: " + searchResult.team1MVP.points + "; Rebounds: " + searchResult.team1MVP.rebounds + "; Assists: " + searchResult.team1MVP.assist + "; Blocks: " + searchResult.team1MVP.blocks} />
        </ListItem>
        <ListItem>
          <ListItemText primary="Team 2 MVP" secondary={"Name: " + searchResult.team2MVP.name + "; Points: " + searchResult.team2MVP.points + "; Rebounds: " + searchResult.team2MVP.rebounds + "; Assists: " + searchResult.team2MVP.assist + "; Blocks: " + searchResult.team2MVP.blocks} />
        </ListItem>
        <Divider component="li" />
        <li>
          <Typography
            sx={{ mt: 0.5, ml: 9 }}
            color="text.secondary"
            display="block"
            variant="caption"
          >
            Related Tweets
          </Typography>
        </li>
      </List>
  }
  // if (searchResult !== null) {
  //     result = <Box >
  //         <Box sx={{ display: "flex", flexDirection: "column", border: "0.5px solid grey", width: "600px" }}>
  //             <Chip label ="Team1"/>: {searchResult.team1 ?? ""}
  //             <FormLabel>Team2: {searchResult.team2 ?? ""}</FormLabel>
  //             <FormLabel>Team1Score: {searchResult.team1Score ?? ""}</FormLabel>
  //             <FormLabel>Team2Score: {searchResult.team2Score ?? ""}</FormLabel>
  //             <FormLabel>Team1MVP: {searchResult.team1MVP.name ?? ""}</FormLabel>
  //             <FormLabel>Team2MVP: {searchResult.team2MVP.name ?? ""}</FormLabel>
  //             <FormLabel>OtherInfo: {searchResult.otherInfo ?? ""}</FormLabel>
  //         </Box>
  //         <Box sx={{ display: "flex", flexDirection: "column", border: "0.5px solid grey", width: "600px" }}>
  //             <FormLabel>RelatedTweets:</FormLabel>
  //         </Box>

  //     </Box>
  // }
  return (
    result
  )
}

export { SearchResultTemplate1 };