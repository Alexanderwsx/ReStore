// Importing necessary hooks and types from React and the Basket model.
import { PropsWithChildren, createContext, useContext, useState } from "react";
import { Basket } from "../models/basket";

// This interface defines the shape of our context. It's like a contract that
// describes what values and functions our store context will have.
interface StoreContextValue {
  basket: Basket | null; // The current state of the basket.
  setBasket: (basket: Basket) => void; // Function to update the basket.
  removeItem: (productId: number, quantity: number) => void; // Function to remove items from the basket.
}

// Create a new context for our store. This will be used by components to access
// the basket state and functions. The default value is set as undefined.
export const StoreContext = createContext<StoreContextValue | undefined>(
  undefined
);

// Custom hook to use our store context. This simplifies the process of accessing
// the context in components and also provides a safety check.
export function useStoreContext() {
  const context = useContext(StoreContext); // Access the store context.

  // Check if the component using this hook is wrapped inside a StoreProvider.
  // If not, throw an error.
  if (context === undefined) {
    throw new Error("oopt we do not see to be indide a provider");
  }

  // If everything is fine, return the context value.
  return context;
}

// The provider component. This wraps other components and provides them with access
// to the store's state and functions.
export function StoreProvider({ children }: PropsWithChildren<any>) {
  // Initialize the basket state. Initially, the basket is empty (null).
  const [basket, setBasket] = useState<Basket | null>(null);

  // Function to remove items from the basket.
  function removeItem(productId: number, quantity: number) {
    if (!basket) return; // If the basket is empty, exit the function.

    // Create a copy of the current basket items.
    const items = [...basket.items];

    // Find the index of the item with the given productId in our items array.
    const itemIndex = items.findIndex((i) => i.productId === productId);

    // If the item is found and its index is greater than or equal to 0...
    if (itemIndex >= 0) {
      items[itemIndex].quantity -= quantity; // Reduce the item's quantity.

      // If the item's quantity is reduced to zero, remove it from the basket.
      if (items[itemIndex].quantity === 0) items.splice(itemIndex, 1);

      // Update the basket state with the modified items.
      setBasket((prevState) => {
        return { ...prevState!, items };
      });
    }
  }

  // Render the StoreContext.Provider component and provide the basket state
  // and functions as its value. This allows child components to access and
  // manipulate the basket's state.
  return (
    <StoreContext.Provider value={{ basket, setBasket, removeItem }}>
      {children}
    </StoreContext.Provider>
  );
}
