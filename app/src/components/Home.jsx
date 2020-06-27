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
            breaks: null,
            breaksWithCommercials: null,
            total : 0
        }

        this.ensureDataFetched = this.ensureDataFetched.bind(this);
        this.refetchData = this.refetchData.bind(this);
        this.setLoading = this.setLoading.bind(this);
    }


    setLoading(isLoading) {
        this.setState({ isLoading });
    }



    componentDidMount() {
        this.ensureDataFetched();
    }

    ensureDataFetched() {
        this.setLoading(true);
        const client = new httpClient();
        return client.get(`${config.aws.api.url}${config.aws.api.path}`, { 'x-api-key': config.aws.api.key })
            .then(response => {
                if (response.data && response.status === 200) {
                    this.setState((state) => {
                        return { state: state.breaks = response.data };
                    });
                }
                this.setLoading(false);
            })
            .catch((err) => { this.setLoading(false); });
    }

    refetchData() {
        this.setLoading(true);
        const client = new httpClient();
        return client.post(`${config.aws.api.url}${config.aws.api.path}`, this.state.breaks, { 'x-api-key': config.aws.api.key })
            .then(response => {
                if (response.data && response.status === 200) {
                    this.setState((state) => {
                        return { state: state.breaksWithCommercials = response.data.breaksWithCommercials, total: response.data.total };
                    });
                }
                this.setLoading(false);
            })
            .catch((err) => { this.setLoading(false); });
    }

    updateRating(value, brId, demoName) {
        const breaks = this.state.breaks;
        breaks.map(p => p.Id === brId ? p.Ratings.map(d => d.DemoName === demoName ? d.Score = value : d) : p);
        this.setState((state) => {
            return { state: state.breaks = breaks };
        })
    }


    render() {
        return (
            <React.Fragment>
                <div className="row">
                    <div className="col-md-4">Break Id</div><div className="col-md-6">Commercial Demographic</div><div className="col-md-2">Rating</div>
                </div>
                {this.state.breaks && this.state.breaks.map((br, i) =>

                    <div key={i} className="row">
                        <div key={i + 1} className="col-md-4" >{br.Id}</div>

                        <div key={i + 2} className="col-md-6">
                            {br.Ratings.map((rating, j) =>
                                <div key={j} className="col-md-12">{rating.DemoName}</div>
                            )}
                        </div>

                        <div key={i + 3} className="col-md-2">
                            {br.Ratings.map((rating, k) =>
                                <input key={k + 1} type="text" onChange={(e) => this.updateRating(parseInt(e.target.value, 10), br.Id, rating.DemoName)} className="col-md-12" defaultValue={rating.Score} placeholder={rating.Score} />
                            )}
                        </div>

                    </div>
                )}

                <div className="row">
                    <div className="col-md-2">Break Id</div><div className="col-md-2">Commercial Id</div><div className="col-md-4">Commercial Demographic</div><div className="col-md-4">Commercial Type</div>
                </div>

                {this.state.breaksWithCommercials && this.state.breaksWithCommercials.map((br, i) =>
                    <div key={i} className="row">
                        <div key={i + 'a'} className="col-md-2" >{br.Id}</div>


                        <div key={i + 'b'} className="col-md-2">
                            {br.Commercials && br.Commercials.map((comm, j) =>
                                <div key={j} className="col-md-12">{comm.Id}</div>
                            )}
                        </div>


                        <div key={i + 'c'} className="col-md-4">
                            {br.Commercials && br.Commercials.map((comm, k) =>
                                <div key={k} type="text" className="col-md-12">{comm.TargetDemoName}</div>
                            )}
                        </div>


                        <div key={i + 'd'} className="col-md-4">
                            {br.Commercials && br.Commercials.map((comm, l) =>
                                <div key={l} type="text" className="col-md-12">{comm.CommercialTypeName}</div>
                            )}
                        </div>


                    </div>
                )}

                            {this.state.total > 0 && <h2>{this.state.total}</h2>}

                {this.state.isLoading
                    ? <Spinner isLoading={this.state.isLoading} />
                    : <input type="button" onClick={this.refetchData} value="optimize"></input>}

            </React.Fragment>
        )
    };
};

export default Home;