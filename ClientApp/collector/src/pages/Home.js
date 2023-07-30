import { useContext } from "react";
import { userContext } from "../context/userContext";

function Home() {
    const { user, setUser } = useContext(userContext);

    return (
        <div>
            <h1>Home page</h1>
            <pre>{user}</pre>
        </div>
    );
}

export default Home;