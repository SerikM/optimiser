import * as React from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavItem, NavLink } from 'reactstrap';
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

                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={this.state.isOpen} navbar>
                            <ul className="navbar-nav flex-grow">
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                                </NavItem>
                            </ul>
                        </Collapse>
                    </Container>
                </Navbar>
            </header>
        );
    }
}

export default NavMenu;