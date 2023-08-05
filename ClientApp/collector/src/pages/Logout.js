import { Link } from "react-router-dom";

const Logout = () => {
    return (
        <div>
            <h1>You have logged out</h1>
            <Link to="/login">click here to login</Link>
        </div>
    );
};
export default Logout;