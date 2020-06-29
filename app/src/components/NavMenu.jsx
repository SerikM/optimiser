import * as React from 'react';
import { Container, Navbar, NavbarBrand } from 'reactstrap';
import { Link } from 'react-router-dom';
import '../index.css';


class NavMenu extends React.PureComponent {
    constructor(props) 
    {
        super(props);
        this.state = 
        { }
    }

    render() {
        return (
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" dark>
                    <Container>
                        <NavbarBrand tag={Link} to="/">Commercial Optimiser</NavbarBrand>
                    </Container>
                </Navbar>
            </header>
        );
    }
}

export default NavMenu;