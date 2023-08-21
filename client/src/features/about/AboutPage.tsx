import {
  Alert,
  AlertTitle,
  Button,
  ButtonGroup,
  Container,
  List,
  ListItem,
  ListItemText,
  Typography,
} from "@mui/material";
import agent from "../../app/api/agent";
import { useState } from "react";

export default function AboutPage() {
  const [validationErros, setValidationErrors] = useState<string[]>([]);

  function getValidationError() {
    agent.TestErros.getValidationError()
      .then(() => console.log("Should not see this"))
      .catch((error) => {
        setValidationErrors(error);
      });
  }

  return (
    <Container>
      <Typography gutterBottom variant="h3">
        Errors for testing purposes
      </Typography>
      <ButtonGroup fullWidth>
        <Button
          variant="contained"
          onClick={() =>
            agent.TestErros.get400Error().catch((error) => console.log(error))
          }
        >
          Test 400 error
        </Button>
        <Button
          variant="contained"
          onClick={() =>
            agent.TestErros.get401Error().catch((error) => console.log(error))
          }
        >
          Test 401 error
        </Button>
        <Button
          variant="contained"
          onClick={() =>
            agent.TestErros.get404Error().catch((error) => console.log(error))
          }
        >
          Test 404 error
        </Button>
        <Button
          variant="contained"
          onClick={() =>
            agent.TestErros.get500Error().catch((error) => console.log(error))
          }
        >
          Test 500 error
        </Button>
        <Button variant="contained" onClick={getValidationError}>
          Test Validation error
        </Button>
      </ButtonGroup>
      {validationErros.length > 0 && (
        <Alert severity="error">
          <AlertTitle>Validation Errors</AlertTitle>
          <List>
            {validationErros.map((error, i) => (
              <ListItem key={error}>
                <ListItemText>{error}</ListItemText>
              </ListItem>
            ))}
          </List>
        </Alert>
      )}
    </Container>
  );
}
