import { Link } from "react-router-dom";

const CreatedCollection = () => {
    return (
        <>
            <h1>Your collection was created</h1>
            <Link to='/collections'>Back to my collections</Link>
            <br /><Link to="/">Home</Link><br />
        </>
    );
}

export default CreatedCollection;