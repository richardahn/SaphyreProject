import React, { Component, useState, useEffect } from 'react';
import axios from 'axios';
import { Alert } from 'reactstrap';
import { hubUrl } from '../global';
import { getHubClient } from '../utils/hubClientFactory';

export function Home() {
  const [editing, setEditing] = useState(false);
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');

  const [editFirstName, setEditFirstName] = useState('');
  const [editLastName, setEditLastName] = useState('');
  const [editPhoneNumber, setEditPhoneNumber] = useState('');

  const [connection, setConnection] = useState(null);

  const [notificationVisible, setNotificationVisible] = useState(false);
  const [notificationMessage, setNotificationMessage] = useState('');

  const onClickEdit = () => {
    setEditFirstName(firstName);
    setEditLastName(lastName);
    setEditPhoneNumber(phoneNumber);
    setEditing(true);
  }
  const onClickCancel = () => {
    setEditFirstName('');
    setEditLastName('');
    setEditPhoneNumber('');
    setEditing(false);
  }
  const onClickSave = () => {
    axios.post('/api/profile', {
      firstName: editFirstName,
      lastName: editLastName,
      phoneNumber: editPhoneNumber
    })
      .then(() => onClickCancel());
  }

  useEffect(() => {
    const newConnection = getHubClient(hubUrl);
    setConnection(newConnection);
  }, []);
  useEffect(() => {
    if (connection) {

      connection.on('DispatchedProfile', (profile, info) => {
        setFirstName(profile.firstName);
        setLastName(profile.lastName);
        setPhoneNumber(profile.phoneNumber);

        setNotificationVisible(true);
        setNotificationMessage(info.message);
      });

      connection.on('FetchedProfile', (profile) => {
        setFirstName(profile.firstName);
        setLastName(profile.lastName);
        setPhoneNumber(profile.phoneNumber);
      })

      connection.start()
        .then(() => connection.send('FetchProfile'))
        .catch(e => console.log('Connection failed: ', e));
    }
  }, [connection])

  return (
    <div>
      {
        notificationVisible &&
        <Alert color="info" isOpen={notificationVisible} toggle={() => setNotificationVisible(false)}>
          {notificationMessage}
    </Alert>
      }
      <h1>Profile</h1>
      {
        editing ?
          (
            <div>
              <div className="form-group">
                <label htmlFor="firstName">First Name:</label>
                <input className="form-control" id="firstName" name="firstName" value={editFirstName} onChange={e => setEditFirstName(e.target.value)} />
              </div>
              <div className="form-group">
                <label htmlFor="lastName">Last Name:</label>
                <input className="form-control" id="lastName" name="lastName" value={editLastName} onChange={e => setEditLastName(e.target.value)} />
              </div>
              <div className="form-group">
                <label htmlFor="phoneNumber">Phone Number:</label>
                <input className="form-control" id="phoneNumber" name="phoneNumber" value={editPhoneNumber} onChange={e => setEditPhoneNumber(e.target.value)} />
              </div>
            </div>
          ) :
          (
            <div>
              <div className="form-group">
                <h5>First Name:</h5>
                <div>{firstName}</div>
              </div>
              <div className="form-group">
                <h5>Last Name:</h5>
                <div>{lastName}</div>
              </div>
              <div className="form-group">
                <h5>Phone Number:</h5>
                <div>{phoneNumber}</div>
              </div>
            </div>
          )
      }
      {
        editing ? (
          <div>
            <button className="btn btn-light" type="button" onClick={onClickCancel} style={{ marginRight: '1rem' }}>Cancel</button>
            <button className="btn btn-success" type="button" onClick={onClickSave}>Save</button>
          </div>
        ) : <button className="btn btn-primary" type="button" onClick={onClickEdit}>Edit</button>
      }
    </div>
  )
}