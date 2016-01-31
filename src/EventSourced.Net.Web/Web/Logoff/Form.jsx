import { reduxForm } from 'redux-form'

class Form extends React.Component {
  static propTypes = {
    handleSubmit: React.PropTypes.func.isRequired,
    submitting: React.PropTypes.bool.isRequired,
    username: React.PropTypes.string,
  };

  render() {
    const { username, handleSubmit, submitting } = this.props
    return(
      <form method="post" onSubmit={handleSubmit}>
        <ul className="nav navbar-nav navbar-right">
          <li>
            <a href="#/not-implemented" title="Manage">Hello {username}!</a>
          </li>
          <li>
            <button type="submit" className="btn btn-link navbar-btn navbar-link" disabled={submitting}>Log off</button>
          </li>
        </ul>
      </form>
    )
  }
}

const form = 'logoff'
const fields = ['returnUrl']
export default reduxForm({
  form,
  fields,
})(Form)
