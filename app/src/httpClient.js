import axios from 'axios';

class httpClient {
  
  handleServerError = err => {
    return Promise.reject(err);
  };

  post(url, request, headers) {
    return axios.post(url, request, { headers: headers }).catch(err =>
      this.handleServerError(err)
    );
  }

  get(url, headers) {
    return axios.get(url, { headers: headers }).catch(err =>
      this.handleServerError(err)
    );
  }

  put(url, request, headers) {
    return axios.put(url, request, { headers: headers }).catch(err =>
      this.handleServerError(err)
    );
  }

  remove(url, headers) {
    return axios.delete(url, { headers: headers }).catch(err =>
      this.handleServerError(err)
    );
  }

  dataFetcher(url, data) {
    return axios({
      url,
      method: data ? 'POST' : 'GET',
      data,
      withCredentials: true
    });
  }

}

export default httpClient;
