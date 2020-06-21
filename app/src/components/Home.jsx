import * as React from 'react';
import '../index.css';
import httpClient from '../httpClient';
import Spinner from './Spinner';
const config = require('../config.json');


class Home extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            isLoading: false,
            data: null
        }
        
        this.ensureDataFetched = this.ensureDataFetched.bind(this);
        this.setLoading = this.setLoading.bind(this);
    }


     setLoading (isLoading){
        this.setState({ isLoading });
    }



    componentDidMount() {
    }

    ensureDataFetched() {

    this.setLoading(true);

        const client = new httpClient();
        return client.get(`${config.aws.api.url}${config.aws.api.path}`, { 'x-api-key': config.aws.api.key })
            .then(response => {
                const data = JSON.parse(response.data.body);

                if (data && response.data && response.status === 200) {

                    this.setState((state) => {
                        return { state: state.data = data };
                    });
                }
                this.setLoading(false);
            }).catch((err) => {this.setLoading(false);});
    }

    render() {
        return (
            <React.Fragment>
                
                {this.state.isLoading 
                ?  <Spinner isLoading={this.state.isLoading} />
                :  <input type="button" onClick={this.ensureDataFetched} value="get data"></input>}
            </React.Fragment>
        )
    };
};

export default Home;