import { useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";

const LiveChart = () => {
  const [price, setPrice] = useState(null);
  
  useEffect(() => {
  const connection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost:5238/cryptoHub")
  .configureLogging(signalR.LogLevel.Information)
  .build();
  connection.start().catch((err) => console.error(err.toString()));

connection.on("ReceivePriceUpdate", (updatedPrice) => {
  setPrice(updatedPrice);
});

return () => {
  connection.stop();
};
}, []);

return (
<div>
<h3>BTC/USDT Live Chart</h3>
{price ? (
<p>Current Price: {price}</p>
) : (
<p>Loading...</p>
)}
</div>
);
};

export default LiveChart;